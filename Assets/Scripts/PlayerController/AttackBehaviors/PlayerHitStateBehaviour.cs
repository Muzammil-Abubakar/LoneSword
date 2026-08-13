using UnityEngine;

public class PlayerHitStateBehaviour : StateMachineBehaviour
{
    [Header("Hit Reaction")]
    [SerializeField] private float recoveryTime = 0.9f;

    private PlayerAnimatorController player;
    private bool recoveryStarted;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        player = animator.GetComponent<PlayerAnimatorController>();

        recoveryStarted = false;

        Debug.Log("HIT STATE ENTER");
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (player == null || recoveryStarted)
        {
            return;
        }

        float time = stateInfo.normalizedTime;

        if (time >= recoveryTime)
        {
            recoveryStarted = true;

            player.EndHitReaction();

            Debug.Log("HIT RECOVERY");
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        Debug.Log("HIT STATE EXIT");
    }
}