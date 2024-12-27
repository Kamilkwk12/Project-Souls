using UnityEngine;

public class DeathState : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInChildren<SpriteRenderer>().enabled = false;
        animator.GetComponent<CapsuleCollider2D>().enabled = true;
    }
}
