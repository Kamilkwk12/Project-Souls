using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAttackState : StateMachineBehaviour
{

    BoxCollider2D boxCollider;
    List<Collider2D> hitResults = new List<Collider2D>();
    PlayerStatus _playerStatus;
    int _attackDmg;

    bool _wasPlayerHit = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        boxCollider = animator.gameObject.GetComponentInChildren<BoxCollider2D>();
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        _attackDmg = animator.GetComponent<EnemyAI>().AttackDamage;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_wasPlayerHit) { 
            Physics2D.OverlapCollider(boxCollider, hitResults);
        }

        if (hitResults.Count > 0)
        {
            foreach (Collider2D collision in hitResults)
            {
                if (collision.gameObject.CompareTag("Player") == true && !_wasPlayerHit)
                {
                    _playerStatus.TakeDamage(_attackDmg);
                    _wasPlayerHit = true;
                }
            }
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitResults.Clear();
        _wasPlayerHit = false;
    }
}
