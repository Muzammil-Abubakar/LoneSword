/*using UnityEngine;

public class SkeletonAttackState : StateMachineBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private float hitStart = 0.25f;
    [SerializeField] private float hitEnd = 0.55f;

    private SkeletonAI skeleton;
    private SkeletonAttackHitbox attackHitbox;

    private bool hitboxEnabled;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        skeleton = animator.GetComponent<SkeletonAI>();

        attackHitbox =
            animator.GetComponentInChildren<SkeletonAttackHitbox>(
                true
            );

        hitboxEnabled = false;

        if (attackHitbox == null)
        {
            Debug.LogError(
                $"{animator.gameObject.name} could not find SkeletonAttackHitbox."
            );
        }
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        float time = stateInfo.normalizedTime;

        bool shouldBeActive =
            time >= hitStart &&
            time < hitEnd;

        if (shouldBeActive && !hitboxEnabled)
        {
            hitboxEnabled = true;
            attackHitbox.EnableHitbox();

          
        }
        else if (!shouldBeActive && hitboxEnabled)
        {
            hitboxEnabled = false;
            attackHitbox.DisableHitbox();

            
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        // Safety: always disable the hitbox.
        if (hitboxEnabled)
        {
            hitboxEnabled = false;

            if (attackHitbox != null)
            {
                attackHitbox.DisableHitbox();
            }
        }

        // Tell SkeletonAI the attack is finished.
        if (skeleton != null)
        {
            skeleton.EndAttack();
        }
    }
}*/