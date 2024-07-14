using UnityEngine;
using TarodevController;
using System.Collections;

public class BreakableWall : MonoBehaviour
{
    public GameObject platform; // Reference to the platform GameObject with the Explodable script
    public GameObject platformPrefab; // Prefab of the platform to instantiate after explosion
    public float instantiateDelay = 2.0f; // Delay in seconds before instantiating the new platform
    public float explodeDelay = 0.5f; // Delay in seconds before instantiating the new platform
    private PlayerController playerController;
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    private GameObject player; // Reference to the player GameObject
    private bool isTriggerRunning = false; // Bool to check if the trigger is currently running

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger detected with: " + collider.gameObject.name);

        // Check if the trigger is currently running to prevent multiple executions
        if (isTriggerRunning)
        {
            return;
        }

        // Check if the collider is the player and if the player is on the wall
        if (collider.gameObject == player && (playerController._grounded|| playerController._isOnWall))
        {
            Debug.Log("Player triggered the platform.");
            isTriggerRunning = true; // Set the flag to true to indicate the trigger is running
            StartCoroutine(ExplodePlatformAfterDelay());
            //StartCoroutine(InstantiatePlatformAfterDelay());
        }
    }

    private IEnumerator InstantiatePlatformAfterDelay()
    {
        Debug.Log("Starting InstantiatePlatformAfterDelay coroutine.");
        // Wait for the specified delay
        yield return new WaitForSeconds(instantiateDelay);

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
        isTriggerRunning = false; // Reset the flag after the platform is instantiated
        Debug.Log("BoxCollider2D re-enabled.");
    }

    private IEnumerator ExplodePlatformAfterDelay()
    {
        Debug.Log("Starting ExplodePlatformAfterDelay coroutine.");
        yield return new WaitForSeconds(explodeDelay);
        ExplodePlatform();
        boxCollider.enabled = false;
        Debug.Log("BoxCollider2D disabled after explosion.");
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