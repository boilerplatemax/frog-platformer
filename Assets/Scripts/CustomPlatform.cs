using UnityEngine;
using TarodevController; // Replace with your actual namespace for PlayerController
using System.Collections;

public class CustomPlatform : MonoBehaviour
{
    
    public GameObject platform; // Assign in the Inspector
    public float velocityThreshold = 0.1f; // Threshold to determine upward movement (adjust as needed)
    public float disableEnableDuration = 0.5f; // Duration to disable and enable the platform (adjust as needed)
    public float upwardForce = 5.0f; // Adjust the force as needed

    private GameObject player;
    private PlayerController playerController;
    private bool isPressingDown;
    private bool isOnWall;
    private bool wasMovingUp = false;
    private Collider2D platformCollider;
    private int playerLayer;
    private int ballLayer;
    private int platformLayer;

    void Awake(){
        playerLayer = LayerMask.NameToLayer("Player");
        ballLayer = LayerMask.NameToLayer("Ball");
        platformLayer = LayerMask.NameToLayer("OneWayPlatform");
    }

    void Update(){
        //if player is below the gameobject platform then enable the platforms transform to true.
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }

        platformCollider = platform.GetComponent<Collider2D>();

        if (platformCollider == null)
        {
            Debug.LogError("Platform GameObject does not have a Collider2D component.");
        }
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            

            
            
            if (playerController.Velocity.y > velocityThreshold)
            {
                
                // Add upward force to the player
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);
                }
            }
            // Additional conditions
            isPressingDown = playerController.Input.y == -1;
        isOnWall = playerController._isOnWall;
        if (isPressingDown)
        {
            StartCoroutine(DisableEnablePlatformCoroutine());
            Debug.Log("Down pressed and Grounded");
        }

        if (isOnWall)
        {
            platform.SetActive(false);
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            Physics2D.IgnoreLayerCollision(ballLayer, platformLayer, true);
            
        }
        }
        
    }
    void OnTriggerEnter2D(Collider2D other){
        

        // Check if player is moving upwards based on velocity
        bool isMovingUp = playerController.Velocity.y > velocityThreshold;

        // Toggle platform visibility based on movement direction change
        if (isMovingUp && !wasMovingUp)
        {
            // Player is moving up, disable platform
            platform.SetActive(false);
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            Physics2D.IgnoreLayerCollision(ballLayer, platformLayer, true);
        }
        else if (!isMovingUp && wasMovingUp)
        {
            // Player is moving down or stationary, enable platform
            platform.SetActive(true);
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
            Physics2D.IgnoreLayerCollision(ballLayer, platformLayer, false);
        }

        // Update the previous state
        wasMovingUp = isMovingUp;

        
    }
    

    IEnumerator DisableEnablePlatformCoroutine()
    {
        platformCollider.enabled = false;
        yield return new WaitForSeconds(disableEnableDuration);
        platformCollider.enabled = true;
    }
}
