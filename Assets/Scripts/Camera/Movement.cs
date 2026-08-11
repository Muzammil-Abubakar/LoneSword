using UnityEngine;

public class CameraLockRotation : MonoBehaviour
{
    private Quaternion fixedRotation;

    void Start()
    {
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = fixedRotation;
    }
}