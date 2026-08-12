using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonCombat : MonoBehaviour
{
    
    public void TakeHit()
    {
        Debug.Log($"{gameObject.name} was hit!");
    }
}