using UnityEngine;

public class DummyPlayerLogic : MonoBehaviour
{
    public float lerpDuration = 1.0f; // Public float for the duration
    private Rigidbody2D rb;
    private Animator animator;
    private float lerpTime = 0.0f;
    private bool isLerping = false;

    public ParticleSystem landingParticles; // Public ParticleSystem for landing particles

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("Grounded", false); // Ensure the grounded bool is false on start
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8 || collision.gameObject.layer == 0)
        {
            // Apply all Rigidbody2D constraints
            // rb.constraints = RigidbodyConstraints2D.FreezeAll;

            // Play the landing particles
            if (landingParticles != null)
            {
                landingParticles.Play();
            }

            // Set the grounded bool in the Animator component to true
            animator.SetBool("Grounded", true);
        }
    }
}
