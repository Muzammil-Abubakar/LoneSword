using UnityEngine;

public class SkeletonHitStateBehaviour : StateMachineBehaviour
{
    private SkeletonCombat skeletonCombat;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        skeletonCombat =
            animator.GetComponent<SkeletonCombat>();
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        skeletonCombat.FinishHitReaction();
    }
}