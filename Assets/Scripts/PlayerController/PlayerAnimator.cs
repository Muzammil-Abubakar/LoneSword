using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError(
                "PlayerAnimator could not find Animator."
            );
        }
    }

    public void PlaySlash()
    {
        animator.SetTrigger("Slash1");
    }

    public void PlayHit()
    {
        animator.SetTrigger("Hit");
    }

    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat(
            "Speed",
            speed,
            0.15f,
            Time.deltaTime
        );
    }

    public void StopMovementAnimation()
    {
        animator.SetFloat("Speed", 0f);
    }
}