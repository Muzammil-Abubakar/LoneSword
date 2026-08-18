using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement State")]
    [SerializeField] private bool canMove = true;

    public Transform Target => target;

    public bool CanMove => canMove;

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
    }

    private void ValidateReferences()
    {
        if (target == null)
        {
            Debug.LogError(
                $"{nameof(EnemyManager)} requires a target reference.",
                this
            );
        }
    }

    private void Awake()
    {
        ValidateReferences();
    }
}