using UnityEngine;

public class PlayerHitStateBehaviour : StateMachineBehaviour
{
    [Header("Hit Reaction")]
    [SerializeField] private float recoveryTime = 0.9f;

    private PlayerManager playerManager;
    private bool recoveryStarted;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        playerManager = animator.GetComponent<PlayerManager>();

        recoveryStarted = false;

        Debug.Log("HIT STATE ENTER");
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (playerManager == null || recoveryStarted)
        {
            return;
        }

        float time = stateInfo.normalizedTime;

        if (time >= recoveryTime)
        {
            recoveryStarted = true;

            playerManager.EndHitReaction();

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