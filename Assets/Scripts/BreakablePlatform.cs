using UnityEngine;
using TarodevController;
using System.Collections;

public class BreakablePlatform : MonoBehaviour
{
    public GameObject platform; // Reference to the platform GameObject with the Explodable script
    public GameObject platformPrefab; // Prefab of the platform to instantiate after explosion
    public float instantiateDelay = 2.0f; // Delay in seconds before instantiating the new platform
    public float explodeDelay = 0.5f; // Delay in seconds before instantiating the new platform

    private PlayerController playerController;
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    private GameObject player; // Reference to the player GameObject

    private bool isCoroutineRunning = false;
    private bool isColliding = false;

    private void Start()
    {
        // Get the BoxCollider2D component attached to this GameObject
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true; // Set the collider as a trigger
        Debug.Log("BoxCollider2D attached to platform found and set as trigger.");
        
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            Debug.Log("Player GameObject found.");
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure the player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (isColliding)
        {
            if(playerController.Velocity.y < 0.1f && !isCoroutineRunning)StartCoroutine(ExplodePlatformAfterDelay());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger detected with: " + collider.gameObject.name);
        if (collider.gameObject == player)
        {
            isColliding = true;
            Debug.Log("Player triggered the platform.");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            isColliding = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            int contactCount = collider.GetContacts(contacts);

            for (int i = 0; i < contactCount; i++)
            {
                ContactPoint2D contact = contacts[i];
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f)
                {
                    isColliding = true;
                    return;
                }
            }
            isColliding = false;
        }
    }

    private void InstantiatePlatformAfterDelay()
    {
        Debug.Log("Starting InstantiatePlatformAfterDelay coroutine.");
        // Wait for the specified delay

        // Instantiate the new platform prefab
        if (platformPrefab != null)
        {
            GameObject newPlatform = Instantiate(platformPrefab, transform.position, Quaternion.identity);
            newPlatform.transform.parent = this.gameObject.transform;
            platform = newPlatform;
            Debug.Log("New platform instantiated.");
        }
        else
        {
            Debug.LogWarning("platformPrefab is not assigned.");
        }
        
        // Enable the BoxCollider2D again
        boxCollider.enabled = true;
        Debug.Log("BoxCollider2D re-enabled.");
    }

    private IEnumerator ExplodePlatformAfterDelay()
    {
        isCoroutineRunning = true;
        Debug.Log("Starting ExplodePlatformAfterDelay coroutine.");
        yield return new WaitForSeconds(explodeDelay);
        ExplodePlatform();
        boxCollider.enabled = false;
        Debug.Log("BoxCollider2D disabled after explosion.");
        yield return new WaitForSeconds(instantiateDelay);
        InstantiatePlatformAfterDelay();
        isCoroutineRunning = false;
    }

    private void ExplodePlatform()
    {
        // Check if platform is valid and has the Explodable script
        if (platform != null)
        {
            // Example assuming Explodable is a function in another script attached to the platform
            platform.GetComponent<Explodable>().explode();
            Debug.Log("Platform exploded.");
        }
        else
        {
            Debug.LogWarning("Platform or Explodable script not found.");
        }
    }
}
