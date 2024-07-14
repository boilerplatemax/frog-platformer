using UnityEngine;
using System.Collections;
using TarodevController;
using TarodevController.Demo;

public class PlayerCollisionHandler : MonoBehaviour
{
    private Vector3 respawnPoint;
    public int health = 1;
    public bool alive = true;
    public Animator canvasAnimator;

    private PlayerController playerController;

    public Animator playerAnimator;
    public float deathWaitTime = 1f;
    public float respawnWaitTime = 2f;

    private bool isHandlingDeath = false;

    public GameObject playerDummyPrefab;
    private GameObject playerDummyInstance;

    public Camera mainCamera;

    public GameObject Particles;

    public GameObject playerSprite; // Public GameObject for the player sprite

    private void Start()
    {
        // Set initial respawn point or default spawn point
        respawnPoint = transform.position;

        // Find the player controller component
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            Debug.Log("PlayerController component found.");
        }

        if (playerSprite == null)
        {
            Debug.LogError("Player sprite GameObject not assigned.");
        }
    }

    public void Hurt()
    {
        playerAnimator.SetTrigger("Hurt");

        health--;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (alive)
        {
            // Set bool alive to false
            alive = false;

            // Start the death handling coroutine
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        if (isHandlingDeath)
            yield break;
        Particles.gameObject.SetActive(false);
        isHandlingDeath = true;

        // Disable the player sprite game object
        if (playerSprite != null)
        {
            playerSprite.SetActive(false);
        }

        // Instantiate the player dummy and match its Rigidbody2D velocity
        if (playerDummyPrefab != null)
        {
            playerDummyInstance = Instantiate(playerDummyPrefab, transform.position, transform.rotation);
            mainCamera.GetComponent<CameraFollow>().AssignNewPlayer(playerDummyInstance);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Rigidbody2D dummyRb = playerDummyInstance.GetComponent<Rigidbody2D>();
            if (rb != null && dummyRb != null)
            {
                dummyRb.velocity = rb.velocity;
            }

            // Flip the sprite X value of the component that is the child of the instantiated dummy player object called "Sprite"
            Transform spriteTransform = playerDummyInstance.transform.Find("Sprite");
            if (spriteTransform != null)
            {
                SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // Flip the X value based on the playerSprite's flip state
                    spriteRenderer.flipX = playerSprite.GetComponent<SpriteRenderer>().flipX;
                }
            }
        }

        // Wait for deathWaitTime seconds
        playerController._alive = false;
        yield return new WaitForSeconds(deathWaitTime);
        canvasAnimator.SetBool("Dead", true);

        // Wait for respawnWaitTime seconds
        yield return new WaitForSeconds(respawnWaitTime);

        // Set player position to the last checkpoint touched
        transform.position = respawnPoint;
        playerController._rb.velocity=Vector2.zero;

        // Optionally reset health and alive status if needed
        health = 1;
        alive = true;
        Particles.gameObject.SetActive(true);

        // Destroy the player dummy that was instantiated earlier
        if (playerDummyInstance != null)
        {
            mainCamera.GetComponent<CameraFollow>().AssignNewPlayer(gameObject);
            Destroy(playerDummyInstance);
        }

        // Enable the player sprite game object that we previously disabled
        if (playerSprite != null)
        {
            playerSprite.SetActive(true);
        }

        // Reset the Dead bool in the animator
        canvasAnimator.SetBool("Dead", false);
        playerController._alive = true;
        isHandlingDeath = false;
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            // Update respawn point to the position of the checkpoint
            SetRespawnPoint(other.transform.position);
            Debug.Log("Checkpoint touched");
        }
        else if (other.CompareTag("Danger"))
        {
            Hurt();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Danger"))
        {
            Hurt();
        }
    }
}
