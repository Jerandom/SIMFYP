using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossEnemy_Proto : MonoBehaviour
{
    public enum State
    {
        CHASE,
        STUNNED,
        CONFUSED,
    }
    public State state;
    [SerializeField]
    private string enemyType;

    public GameObject player;
    public GameObject slash;

    public AIPath AiPath;

    public Animator animator;

    public float waypointDelay = 0;

    IAstarAI agent;

    Vector3 startingPosition;
    Vector3 targetPosition;
    Vector3 dirToTarget;

    public LayerMask layerMask;

    private FieldOfView fovInner;

    public Transform pfFieldofView;

    public float innerfovAngle;
    public float innerfovViewDistance;

    public bool isSpotted;
    public float countdown = 5;
    public float setDestinationCountdown = 1.5f;
    public float alertCountdown = 5f;
    public float stunnedTimer = 5.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.Player);
        slash = transform.GetChild(0).gameObject;

        AiPath = this.GetComponent<AIPath>();

        animator = GetComponent<Animator>();
        agent = GetComponent<IAstarAI>();

        fovInner = Instantiate(pfFieldofView, null).GetComponent<FieldOfView>();

        //set fov layers to FoV for colliders 
        fovInner.gameObject.layer = 9;

        slash = this.transform.GetChild(0).gameObject;

        fovInner.setFovAngle(innerfovAngle);
        fovInner.setViewDistance(innerfovViewDistance);


    }
    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            default:
            case State.CHASE:
                chaseState();
                //FovFindPlayer();

                break;

            case State.STUNNED:
                stunnedState();

                break;

            case State.CONFUSED:
                confusedState();
                FovFindPlayer();

                break;
        }
    }

    private void chaseState()
    {
        float radius = 0.7f;
        agent.destination = player.transform.position;

        if (this.GetComponent<AIPath>().velocity.magnitude > Mathf.Epsilon)
        {
            fovInner.setLookDirection(dirToTarget);
        }
        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        //fovInner.setLookDirection(dirToTarget);

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
        {
            animator.SetBool("isAttack", true);
            Sound.PlaySound(Sound.SoundType.bossSwipe);
        }
        else
            animator.SetBool("isAttack", false);
    }
    private void FovFindPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < innerfovViewDistance)
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
                    if (raycastHit2D.collider.tag == "Player")
                    {
                        //hit player
                        state = State.CHASE;
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

    private void stunnedState()
    {

        AiPath.maxSpeed = 0f;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovInner.setLookDirection(dirToTarget);

        animator.SetFloat("Speed", 0);
        animator.SetBool("isAttack", false);
        stunnedTimer -= Time.deltaTime;

        //bait baitCountdown
        if (stunnedTimer < 0)
        {
            stunnedTimer = 5f;
            state = State.CONFUSED;
            AiPath.maxSpeed = 1f;
        }

        else
        {
            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dirToTarget.x);
            animator.SetFloat("Vertical", dirToTarget.y);
        }
    }

    private void confusedState()
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
            setDestinationCountdown = 1.5f;
        }

        //check if the player is spotted
        if (isSpotted)
        {
            state = State.CHASE;
            alertCountdown = 5f;
        }
        //check if the player is not spotted
        else if (setDestination && isSpotted == false)
        {
            //move in random direction
            agent.destination = getRoamingPosition();
        }

        //alert level Countdown
        if (alertCountdown > 0)
            alertCountdown -= Time.deltaTime;
        //go back patrol if did not detect player
        else if (alertCountdown < 0)
        {
            state = State.CHASE;
            alertCountdown = 5f;
            setDestinationCountdown = 1.5f;
        }

        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

        //set fov to enemy and direction to target
        fovInner.setOrigin(transform.position);
        fovInner.setLookDirection(dirToTarget);

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

    private Vector3 getRoamingPosition()
    {
        return transform.position + getRandomDirection() * UnityEngine.Random.Range(0.01f, 0.01f);
    }

    private Vector3 getRandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}