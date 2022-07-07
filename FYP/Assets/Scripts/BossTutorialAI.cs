using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossTutorialAI : MonoBehaviour
{
    Animator animator;
    IAstarAI agent;
    public AIPath AiPath;
    public GameObject player;
    private Vector3 dirToTarget;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<IAstarAI>();
        gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag(Constants.Player);
    }

    // Update is called once per frame
    void Update()
    {
        float radius = 0.7f;
        agent.destination = player.transform.position;

        //set look direction
        targetPosition = agent.destination;
        dirToTarget = (targetPosition - transform.position).normalized;

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

        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            animator.SetBool("isAttack", true);
            Sound.PlaySound(Sound.SoundType.bossSwipe);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(true);
    }
}
