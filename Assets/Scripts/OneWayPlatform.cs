using System.Collections;
using UnityEngine;
using TarodevController;

public class OneWayPlatform : MonoBehaviour
{
    public bool canGoDown=true;
    private Collider2D platformCollider;
    private GameObject player;
    private PlayerController playerController;
    private bool isCoroutineRunning = false;
    private bool isPressingDown = false;

    private bool grounded;
    
    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            Debug.Log("Player GameObject found.");
        }
    }

    private void Update()
    {
        if (playerController != null)
        {
            isPressingDown = playerController.Input.y == -1;
            grounded = playerController._grounded;
            
            if (playerController.Velocity.y > 3.0f || isCoroutineRunning)
            {
                //Debug.Log("Player velocity above 3.0f, disabling platform collider.");
                platformCollider.enabled = false;
            }
            else
            {
                //Debug.Log("Player velocity below or equal to 3.0f, enabling platform collider.");
                platformCollider.enabled = true;
            }

            if (canGoDown&& grounded && isPressingDown && !isCoroutineRunning)
            {
                StartCoroutine(PassThroughPlatform());
            }
        }
    }

    private IEnumerator PassThroughPlatform()
    {
        isCoroutineRunning = true;

        // Disable collision with the player
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), platformCollider, true);
        Debug.Log("Passing through platform. Collision disabled.");

        // Wait for a short duration to ensure the player has passed through the platform
        yield return new WaitForSeconds(0.5f);

        // Enable collision with the player again
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), platformCollider, false);
        Debug.Log("Passed through platform. Collision enabled.");

        isCoroutineRunning = false;
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Velocity y: " + playerController.Velocity.y);
    //         Vector2 collisionNormal = collision.contacts[0].normal;
    //         Debug.Log($"Collision detected with Player. Collision normal: {collisionNormal}");

    //         // Check if the collision normal is pointing upwards (player is below the platform)
    //         if (collisionNormal.y > 0)
    //         {
    //             Debug.Log("Player is below the platform. Ignoring collision.");
    //             // Ignore collision with the platform
    //             Physics2D.IgnoreCollision(collision.collider, platformCollider, true);
    //         }
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Player exited collision. Re-enabling collision.");
    //         // Re-enable collision with the platform after the player exits collision
    //         Physics2D.IgnoreCollision(collision.collider, platformCollider, false);
    //     }
    // }
}
