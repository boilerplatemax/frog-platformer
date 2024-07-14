using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollisionHandler player = other.GetComponent<PlayerCollisionHandler>();
            if (player != null)
            {
                // Set the respawn point to this checkpoint's position
                player.SetRespawnPoint(transform.position);
            }
        }
    }
}
