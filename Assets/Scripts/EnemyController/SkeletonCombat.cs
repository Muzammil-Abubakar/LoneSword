using UnityEngine;

public class SkeletonCombat : MonoBehaviour
{
    private SkeletonAI skeletonAI;

    private void Awake()
    {
        skeletonAI = GetComponent<SkeletonAI>();
    }

    public void TakeHit()
    {
        Debug.Log($"{gameObject.name} was hit!");

        skeletonAI.StartHitReaction();
    }

    public void FinishHitReaction()
    {
        skeletonAI.EndHitReaction();
    }
}