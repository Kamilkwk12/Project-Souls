using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : StateMachineBehaviour
{
    CircleCollider2D _circleCollider;
    GameObject[] _allEnemies;
    BossStatus _bossStatus;
    List<Collider2D> hitResults = new List<Collider2D>();
    int _attackDmg;
    int _bossResist = 5;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _circleCollider = GameObject.FindGameObjectWithTag("Player Attack").GetComponent<CircleCollider2D>();
        _attackDmg = animator.GetComponent<PlayerStatus>().AttackDamage;
        _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        _bossStatus = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossStatus>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Physics2D.OverlapCollider(_circleCollider, hitResults);

        if (hitResults.Count > 0) {
            foreach (Collider2D collision in hitResults) {
                if (collision.CompareTag("Enemy") == true) {
                    collision.GetComponent<EnemyStatus>().TakeDamage(_attackDmg);
                    collision.GetComponent<EnemyStatus>().CanBeHit = false;
                }

                if (collision.CompareTag("Boss") == true)
                {
                    _bossStatus.TakeDamage(_attackDmg - _bossResist);
                    _bossStatus.CanBeHit = false;
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (GameObject enemy in _allEnemies) {
            enemy.GetComponent<EnemyStatus>().CanBeHit = true;
        }
        _bossStatus.CanBeHit = true;
        hitResults.Clear();
        animator.SetBool("isAttacking", false);
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
