using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private bool isCollected = false;

    private void Start()
    {
        // Get the AudioSource component attached to the coin
        audioSource = GetComponent<AudioSource>();
        // Get the SpriteRenderer component attached to the coin
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Get the Collider2D component attached to the coin
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Call a method to add the coin's value to the player's score
            GameManager.instance.AddScore(coinValue);

            // Play the collect sound
            PlayCollectSound();

            // Disable the sprite renderer and collider to make the coin disappear and prevent re-triggering
            spriteRenderer.enabled = false;
            collider2D.enabled = false;

            // Destroy the coin after a short delay to allow the sound to play
            Destroy(gameObject, audioSource.clip.length);
        }
    }

    private void PlayCollectSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            // Adjust pitch or other properties if needed
            audioSource.pitch = Random.Range(0.8f, 1.2f); // Example: randomize pitch slightly
            audioSource.Play();
        }
    }
}
