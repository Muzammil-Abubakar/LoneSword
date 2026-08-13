using UnityEngine;

public class SkeletonAttackState : StateMachineBehaviour
{
    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        SkeletonAI skeleton = animator.GetComponent<SkeletonAI>();

        if (skeleton != null)
        {
            skeleton.EndAttack();
        }
    }
}