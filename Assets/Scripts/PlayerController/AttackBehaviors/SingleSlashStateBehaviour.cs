using UnityEngine;

public class SingleSlashStateBehaviour : StateMachineBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private float hitStart = 0.25f;
    [SerializeField] private float hitEnd = 0.55f;

    private PlayerAnimatorController player;
    private bool hitboxEnabled;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        player = animator.GetComponent<PlayerAnimatorController>();

        hitboxEnabled = false;
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        float time = stateInfo.normalizedTime;

        bool shouldBeActive =
            time >= hitStart &&
            time < hitEnd;

        if (shouldBeActive && !hitboxEnabled)
        {
            hitboxEnabled = true;
            player.EnableHitbox();
        }
        else if (!shouldBeActive && hitboxEnabled)
        {
            hitboxEnabled = false;
            player.DisableHitbox();
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        // Safety cleanup.
        // The hitbox must never remain active after
        // leaving the attack state.
        if (hitboxEnabled)
        {
            hitboxEnabled = false;
            player.DisableHitbox();
        }

        // Tell the player controller that the
        // attack animation has completely finished.
        player.EndSlash();
    }
}