using UnityEngine;

public class SingleSlashStateBehaviour : StateMachineBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private float hitStart = 0.25f;
    [SerializeField] private float hitEnd = 0.55f;

    private PlayerManager playerManager;

    private bool hitboxEnabled;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        playerManager =
            animator.GetComponent<PlayerManager>();

        hitboxEnabled = false;
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        float time =
            stateInfo.normalizedTime;

        bool shouldBeActive =
            time >= hitStart &&
            time < hitEnd;

        if (shouldBeActive && !hitboxEnabled)
        {
            hitboxEnabled = true;

            playerManager.EnableAttackHitbox();
        }
        else if (!shouldBeActive && hitboxEnabled)
        {
            hitboxEnabled = false;

            playerManager.DisableAttackHitbox();
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        // Safety cleanup.
        if (hitboxEnabled)
        {
            hitboxEnabled = false;

            playerManager.DisableAttackHitbox();
        }

        playerManager.EndAttack();
    }
}