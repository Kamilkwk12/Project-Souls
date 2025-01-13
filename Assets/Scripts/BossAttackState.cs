using System.Collections.Generic;
using UnityEngine;



public class BossAttackState : StateMachineBehaviour
{
    BoxCollider2D boxCollider;
    PlayerStatus _playerStatus;

    bool _wasPlayerHit = false;
    List<Collider2D> hitResults = new List<Collider2D>();
    BossAI _bossAI;
    int _attackDmg;
    int _attackType;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackType = animator.GetInteger("AttackType");

        switch (_attackType)
        {
            case 1:
                boxCollider = GameObject.FindGameObjectWithTag("Boss Attack").GetComponent<BoxCollider2D>();
                break;
            case 2:
                boxCollider = GameObject.FindGameObjectWithTag("Boss Combo").GetComponent<BoxCollider2D>();
                break;
        }
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        _bossAI = animator.GetComponent<BossAI>();
        _attackDmg = _bossAI.AttackDmg;
        animator.SetBool("isAttacking", true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_wasPlayerHit)
        {
            Physics2D.OverlapCollider(boxCollider, hitResults);
        }

        if (hitResults.Count > 0)
        {
            foreach (Collider2D collision in hitResults)
            {
                if (collision.gameObject.CompareTag("Player") == true && !_wasPlayerHit)
                {
                    Debug.Log("Attack " + _attackDmg + " hit " +  collision.gameObject.name);
                    _playerStatus.TakeDamage(_attackDmg);
                    _wasPlayerHit = true;
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking", false);
        hitResults.Clear();
        animator.SetInteger("AttackType", 0);
        _wasPlayerHit = false;
        _bossAI.AttackNumber++;
    }

}
