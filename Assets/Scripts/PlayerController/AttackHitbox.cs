using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private Collider hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
    }
}