using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    List<Collider2D> hitTargets = new List<Collider2D>();

    bool wasEnemyHit = false;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        hitTargets = animator.GetComponent<HitboxManager>().results;

        if (wasEnemyHit || hitTargets.Count <= 0)
        {
            return;
        }

        string hitName = "";

        foreach (Collider2D target in hitTargets)
        {
            if (target.GetType() == typeof(CapsuleCollider2D))
            {
                //hitName = target.name;
                //if (targetStatus != null && )
                //{

                //}

                if (target.CompareTag("Enemy"))
                {
                    EnemyStatus status = target.GetComponent<EnemyStatus>();
                    status.TakeDamage(20);
                } else if (target.CompareTag("Player"))
                {
                    PlayerStatus status = target.GetComponent<PlayerStatus>();
                    status.TakeDamage(20);
                }

                wasEnemyHit = true;
            }
        }

        if (wasEnemyHit)
        {  
            Debug.Log(animator.name + " hit " + hitName);
        }
        hitTargets.Clear();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking", false);
        wasEnemyHit = false;
        hitTargets.Clear();

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
