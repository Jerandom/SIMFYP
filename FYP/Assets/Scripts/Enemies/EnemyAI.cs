using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL,
        CAUTION,
        ALERT,
        BAITED,
        SOUNDWAVE,
    }
    private State state;
    [SerializeField]
    private string enemyType;

    public GameObject[] detectSoundWave;
    public GameObject[] detectBait;
    public GameObject player;
    public GameObject slash;

    public AIPath AiPath;

    public Animator animator;

    public Transform[] waypointTargets;

    public float waypointDelay = 0;

    /// <summary>Current target index</summary>
    int index;

    IAstarAI agent;
    float switchTime = float.PositiveInfinity;

    Vector3 targetPosition;
    Vector3 dirToTarget;

    public LayerMask layerMask;

    private FieldOfView fovInner;
    private FieldOfView fovOuter;

    public Transform pfFieldofView;
    public GameObject pfSoundWave;

    public float innerfovAngle;
    public float innerfovViewDistance;

    public float outerfovAngle;
    public float outerfovViewDistance;

    public bool isSpotted;
    public float baitCountdown = 5f;
    public float soundWaveCountdown = 0.1f;
    public float setDestinationCountdown = 5f;
    public float alertCountdown = 15f;
    public float scoreCountdown = 1f;

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.Player);
        slash = transform.GetChild(0).gameObject;

        AiPath = this.GetComponent<AIPath>();

        animator = GetComponent<Animator>();
        agent = GetComponent<IAstarAI>();

        detectBait = GameObject.FindGameObjectsWithTag("Bait");
        detectSoundWave = GameObject.FindGameObjectsWithTag("Finish");

        fovInner = Instantiate(pfFieldofView, null).GetComponent<FieldOfView>();
        fovOuter = Instantiate(pfFieldofView, null).GetComponent<FieldOfView>();
        
        //set fov layers to FoV for colliders 
        fovInner.gameObject.layer = 9;
        fovOuter.gameObject.layer = 9;

        slash = this.transform.GetChild(0).gameObject;

        SaveSystem.Instance.enemyAIList.Add(this);
    }

    private void Update()
    {
        fovInner.setFovAngle(innerfovAngle);
        fovInner.setViewDistance(innerfovViewDistance);

        fovOuter.setFovAngle(outerfovAngle);
        fovOuter.setViewDistance(outerfovViewDistance);

        //keep on finding the objects with tags
        detectBait = GameObject.FindGameObjectsWithTag("Bait");
        detectSoundWave = GameObject.FindGameObjectsWithTag("SoundWave");

        //cooldown b4 sound wave can be used again
        soundWaveCountdown = Mathf.Max(soundWaveCountdown - Time.deltaTime, 0f);

        //cooldown b4 score can be used again
        scoreCountdown = Mathf.Max(scoreCountdown - Time.deltaTime, 0f);

        switch (state)
        {
            default:

            case State.PATROL:
                patrolState();
                findBait();
                FovFindPlayer();
                checkSoundWaveRadius();

                break;

            case State.CAUTION:
                chaseState();
                FovFindPlayer();

                break;

            case State.ALERT:
                alertState();
                FovFindPlayer();

                break;

            case State.BAITED:
                baitedState();

                break;

            case State.SOUNDWAVE:
                soundWaveState();
                FovFindPlayer();

                break;
        }

    }

    //Commented this out because dont want the bait to destroy instantly if thrown on top of enemy
    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<GroundItem>();
        if (item)   //if it was able to find an item
        {
            Destroy(collision.gameObject);
        }
    }
    */

    private void patrolState()
    {
        //reset the alert and setDestination baitCountdown
        alertCountdown = 15f;
        setDestinationCountdown = 5f;

        if (this.GetComponent<AIPath>().velocity.magnitude > Mathf.Epsilon)
        {
            fovInner.setLookDirection(dirToTarget);
            fovOuter.setLookDirection(dirToTarget);
        }

        if (waypointTargets.Length == 0) return;

        bool search = false;

        // Note: using reachedEndOfPath and pathPending instead of reachedDestination here because
        // if the destination cannot be reached by the agent, we don't want it to get stuck, we just want it to get as close as possible and then move on.
        if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
        {
            switchTime = Time.time + waypointDelay;
        }

        if (Time.time >= switchTime)
        {
            index = index + 1;
            search = true;
            switchTime = float.PositiveInfinity;
        }

        //find the waypoint and go to it
        index = index % waypointTargets.Length;
        agent.destination = waypointTargets[index].position;


        if (search) agent.SearchPath();

        //set look direction
        targetPosition = transform.position + new Vector3(this.GetComponent<AIPath>().velocity.x, this.GetComponent<AIPath>().velocity.y, 0f).normalized;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovOuter.setOrigin(transform.position);
        //fovInner.setLookDirection(dirToTarget);
        //fovOuter.setLookDirection(dirToTarget);

        //animation controls
        if (AiPath.reachedDestination)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dirToTarget.x);
            animator.SetFloat("Vertical", dirToTarget.y);
        }
    }

    private void baitedState()
    {
        //find radius of the bait
        float radius = 2f;
        foreach (GameObject r in detectBait)
        {
            //check if enemy radius is has bait
            if (Vector3.Distance(transform.position, r.transform.position) < radius)
                agent.destination = r.transform.position;
        }

        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovOuter.setOrigin(transform.position);
        fovInner.setLookDirection(dirToTarget);
        fovOuter.setLookDirection(dirToTarget);

        //animation controls
        if (AiPath.reachedDestination)
        {
            animator.SetFloat("Speed", 0);
            baitCountdown -= Time.deltaTime;

            Sound.PlaySound(Sound.SoundType.eatingBait);
            //bait baitCountdown
            if (baitCountdown < 0)
            {
                baitCountdown = 5f;
                state = State.PATROL;
            }
        }
        else
        {
            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dirToTarget.x);
            animator.SetFloat("Vertical", dirToTarget.y);
        }
    }

    private void chaseState()
    {
        bool setDestination = false;

        //checks for set destination, else it will update many times in a single frame
        if (setDestinationCountdown > 0)
        {
            setDestinationCountdown -= Time.deltaTime;
        }
        else
        {
            setDestination = true;
            setDestinationCountdown = 5f;
        }

        //check if the player is spotted
        if (isSpotted)
        {
            agent.destination = player.transform.position;
            alertCountdown = 15f;
        }
        //check if the player is not spotted
        else if(setDestination && isSpotted == false)
        {
            //move in random direction
            agent.destination = getRoamingPosition();
        }
        //check for sound wave if no player detected
        else
        {
            checkSoundWaveRadius();
        }

        //alert level baitCountdown
        if (alertCountdown > 0)
            alertCountdown -= Time.deltaTime;
        //go back patrol if did not detect player
        else if(alertCountdown < 0)
            state = State.PATROL;

        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovOuter.setOrigin(transform.position);
        fovInner.setLookDirection(dirToTarget);
        fovOuter.setLookDirection(dirToTarget);

        //animation controls
        if (AiPath.reachedDestination)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dirToTarget.x);
            animator.SetFloat("Vertical", dirToTarget.y);
        }
    }

    private void alertState()
    {
        float radius = 0.7f;

        generateSoundWave();

        //check if player is detected
        if (isSpotted)
            agent.destination = player.transform.position;
        else
            state = State.CAUTION;

        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovOuter.setOrigin(transform.position);
        fovInner.setLookDirection(dirToTarget);
        fovOuter.setLookDirection(dirToTarget);

        //animation controls
        if (AiPath.reachedDestination)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dirToTarget.x);
            animator.SetFloat("Vertical", dirToTarget.y);
        }
        if (Vector3.Distance(transform.position, player.transform.position) < radius
            && player.GetComponent<PlayerHealth>().getPlayerHP() > 0)
            animator.SetBool("isAttack", true);
        else
            animator.SetBool("isAttack", false);
    }

    private void soundWaveState()
    {
        //check for sound wave radius
        float radius = 3f;
        foreach (GameObject r in detectSoundWave)
        {
            //go to position if there is sound wave in a certain radius
            if (Vector3.Distance(transform.position, r.transform.position) < radius)
                agent.destination = r.transform.position;
            else
                state = State.CAUTION;
        }

        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovInner.setLookDirection(dirToTarget);

        fovOuter.setOrigin(transform.position);
        fovOuter.setLookDirection(dirToTarget);

        //animation controls
        if (AiPath.reachedDestination)
        {
            animator.SetFloat("Speed", 0);

            //if no player detected, switch states
            if(isSpotted == false)
                state = State.CAUTION;
        }
        else
        {
            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dirToTarget.x);
            animator.SetFloat("Vertical", dirToTarget.y);
        }
    }

    private void FovFindPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < outerfovViewDistance 
            && Vector3.Distance(transform.position, player.transform.position) > innerfovViewDistance)
        {
            //player inside outer view distance
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if(Vector3.Angle(dirToTarget, dirToPlayer) < outerfovAngle / 2)
            {
                //player inside view angle
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, outerfovViewDistance, layerMask);
                if(raycastHit2D.collider != null)
                {
                    //hit something
                    //Debug.Log("Hit: " + raycastHit2D.collider.name);
                    if (raycastHit2D.collider.tag == "Player" && player.GetComponent<PlayerHealth>().getPlayerHP() > 0)
                    {
                        //hit player
                        state = State.CAUTION;
                        isSpotted = true;

                        if (scoreCountdown == 0 && isSpotted == true)
                        {
                            scoreCountdown = 1f;
                            ScoreManager.instance.subtractPoint(5);
                        }
                    }
                    else
                    {
                        //hit something else
                        //Debug.Log("null");
                        isSpotted = false;
                    }
                }
            }
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < innerfovViewDistance)
        {
            //player inside inner view distance
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if (Vector3.Angle(dirToTarget, dirToPlayer) < innerfovAngle / 2)
            {
                //player inside view angle
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, innerfovViewDistance, layerMask);
                if (raycastHit2D.collider != null)
                {
                    //hit something
                    //Debug.Log("Hit: " + raycastHit2D.collider.name);
                    if (raycastHit2D.collider.tag == "Player" && player.GetComponent<PlayerHealth>().getPlayerHP() > 0)
                    {
                        //hit player
                        state = State.ALERT;
                        isSpotted = true;
                    }
                    else
                    {
                        //hit something else
                        //Debug.Log("null");
                        isSpotted = false;
                    }
                }
                else
                {
                    //Debug.Log(raycastHit2D.collider);
                }
            }
        }
        else
            isSpotted = false;
    }

    private void checkSoundWaveRadius()
    {
        //sound wave radius
        float radius = 3f;

        //find the bait position and check radius
        foreach (GameObject r in detectSoundWave)
        {
            if (Vector3.Distance(transform.position, r.transform.position) < radius)
                state = State.SOUNDWAVE;
            else
                state = State.PATROL;
        }
    }

    public virtual void findBait()
    {
        //bait radius
        float radius = 2f;

        //find the bait position and check radius
        foreach (GameObject r in detectBait)
        {
            if (Vector3.Distance(transform.position, r.transform.position) < radius)
            {
                Debug.Log(gameObject.name + " has been baited by " + r.name);
                state = State.BAITED;                               //change enemy state to baited
                r.GetComponent<Collider2D>().enabled = false;       //disable collider so player cannot re-pickup the bait once baited
                Destroy(r, 7);                                      //destroy the bait object after 5 seconds
            }
            else
                state = State.PATROL;
        }
    }

    public virtual void generateSoundWave()
    {
        //cooldown for sound wave, if not will spawn sound wave every update
        if (soundWaveCountdown == 0)
        {
            //instantiate sound wave at enemy position
            GameObject newSoundWave = Instantiate(pfSoundWave, transform.position, Quaternion.identity);
            soundWaveCountdown = 5f;

            Destroy(newSoundWave, 1);
        }
    }

    private Vector3 getRoamingPosition()
    {
        return transform.position + getRandomDirection() * UnityEngine.Random.Range(0.01f, -0.01f);
    }

    private Vector3 getRandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    //Destroy enemy object and corresponding FoVs
    public void Die()
    {
        SaveSystem.Instance.enemyAIList.Remove(this);
        Destroy(fovOuter.gameObject);
        Destroy(fovInner.gameObject);
        Destroy(gameObject);
    }

    //return BAITED state to MatchstickUI.cs
    public String getState()
    {
        return state.ToString();
    }

    public State GetState()
    {
        return state;
    }

    public void SetState(State theState)
    {
        state = theState;
    }

    public String getType()
    {
        return enemyType;
    }

    public void setType(string theType)
    {
        enemyType = theType;
    }

    public FieldOfView getInnerFOV()
    {
        return fovInner;
    }

    public FieldOfView getOutterFOV()
    {
        return fovOuter;
    }

    public int getIndex()
    {
        return index;
    }

    public virtual void save(int id)
    {
        string waypoints = "";
        if (waypointTargets.Length > 0)
        {
            for (int i = 0; i < waypointTargets.Length; i++)
            {
                waypoints += "_" + waypointTargets[i].transform.position.ToString();
            }
        }
        PlayerPrefs.SetString(id.ToString(), getType() + "_" + transform.position.ToString() + waypoints);
    }

    public virtual void Load(string[] values)
    {
        transform.localPosition = SaveSystem.Instance.stringToVector(values[1]);
        Array.Clear(waypointTargets, index, waypointTargets.Length);
        for (int i = 2; i < values.Length; i++)
        {
            int offset = 2;
            GameObject go = new GameObject();
            go.name = getType() + (i - offset);
            go.transform.position = SaveSystem.Instance.stringToVector(values[i]);
            waypointTargets[i - offset] = go.transform;
        }
    }

    public virtual void Load2(string[] values)
    {
        waypointTargets = new Transform[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            GameObject go = new GameObject();
            go.name = getType() + (i);
            go.transform.position = SaveSystem.Instance.stringToVector(values[i]);
            waypointTargets[i] = go.transform;
        }
    }

    public void DestroySaveData()
    {
        SaveSystem.Instance.enemyAIList.Remove(this);
    }
}
