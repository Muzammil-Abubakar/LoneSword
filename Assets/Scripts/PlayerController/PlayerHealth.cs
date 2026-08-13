using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerManager playerManager;

    private bool isHit;

    private void Awake()
    {
        playerManager =
            GetComponent<PlayerManager>();

        if (playerManager == null)
        {
            Debug.LogError(
                "PlayerHealth could not find PlayerManager."
            );
        }
    }

    public void TakeHit()
    {
        isHit = true;

        Debug.Log(
            "PLAYER HIT REACTION!"
        );

        playerManager.PlayHit();
    }

    public void EndHitReaction()
    {
        isHit = false;

        Debug.Log(
            "PLAYER HIT REACTION ENDED!"
        );
    }

    public bool IsHit()
    {
        return isHit;
    }
}