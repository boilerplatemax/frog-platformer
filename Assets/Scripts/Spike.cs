using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerCollisionHandler player = collision.collider.GetComponent<PlayerCollisionHandler>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollisionHandler player = other.GetComponent<PlayerCollisionHandler>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
