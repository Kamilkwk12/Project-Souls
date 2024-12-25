using UnityEngine;

public class DeathState : StateMachineBehaviour
{

    GameObject healthbar;
    CapsuleCollider2D collider;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInChildren<SpriteRenderer>().enabled = false;
        animator.GetComponent<CapsuleCollider2D>().enabled = true;
    }
}
