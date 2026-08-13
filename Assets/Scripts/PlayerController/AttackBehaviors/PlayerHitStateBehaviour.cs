using UnityEngine;

public class PlayerHitStateBehaviour : StateMachineBehaviour
{
    private PlayerAnimatorController player;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        player = animator.GetComponent<PlayerAnimatorController>();
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (player != null)
        {
            player.EndHitReaction();
        }
    }
}