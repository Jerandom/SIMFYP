using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnimation : StateMachineBehaviour
{
    Transform player;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    EnemyAI enemyAI;
    float attackRange = 1f;
    [SerializeField]
    Transform slash;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag(Constants.Player).transform;
        slash = animator.transform.GetChild(0);
        rb = animator.GetComponent<Rigidbody2D>();
        enemyAI = animator.GetComponent<EnemyAI>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        slash.position.Set(0,0,0);
        if (Vector2.Distance(player.position, rb.position) <= attackRange && enemyAI.getState() == Constants.Alert)
        {
            //Attack
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
