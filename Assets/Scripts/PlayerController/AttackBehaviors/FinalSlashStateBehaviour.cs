using UnityEngine;

public class FinalSlashStateBehaviour : StateMachineBehaviour
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
        // Always make sure the hitbox is off.
        if (hitboxEnabled)
        {
            hitboxEnabled = false;
            player.DisableHitbox();
        }

        // Slash2 is the end of the attack sequence.
        player.EndSlash();
    }
}