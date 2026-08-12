using UnityEngine;

public class InitialSlashStateBehaviour : StateMachineBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private float hitStart = 0.25f;
    [SerializeField] private float hitEnd = 0.55f;

    [Header("Combo Window")]
    [SerializeField] private float comboStart = 0.60f;
    [SerializeField] private float comboEnd = 0.85f;

    private PlayerAnimatorController player;

    private bool hitboxEnabled;
    private bool comboWindowOpen;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        player = animator.GetComponent<PlayerAnimatorController>();

        hitboxEnabled = false;
        comboWindowOpen = false;
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        float time = stateInfo.normalizedTime;

        UpdateHitbox(time);
        UpdateComboWindow(time);
    }

    private void UpdateHitbox(float time)
    {
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

    private void UpdateComboWindow(float time)
    {
        bool shouldBeOpen =
            time >= comboStart &&
            time < comboEnd;

        if (shouldBeOpen && !comboWindowOpen)
        {
            comboWindowOpen = true;
            player.OpenComboWindow();
        }
        else if (!shouldBeOpen && comboWindowOpen)
        {
            comboWindowOpen = false;
            player.CloseComboWindow();
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

        // Always close the combo window when leaving Slash1.
        if (comboWindowOpen)
        {
            comboWindowOpen = false;
            player.CloseComboWindow();
        }

        // Tell the controller Slash1 is finished.
        player.CompleteSlash1();
    }
}