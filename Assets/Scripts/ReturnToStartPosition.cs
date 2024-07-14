using UnityEngine;
using System.Collections;

public class ReturnToStartPosition : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody2D rb;
    public float returnDelay = 2f;
    public float moveSpeed = 2f; // Adjust this to change the speed of movement

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CancelInvoke("ReturnToStart");
            Invoke("ReturnToStart", returnDelay);
        }
    }

    private void ReturnToStart()
    {
        // Reset any vectors or forces that may be affecting the Rigidbody's momentum
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Start the coroutine to ease into the starting position and rotation
        StartCoroutine(MoveToStartPosition());
    }

    private IEnumerator MoveToStartPosition()
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;

            // Ease into the starting position
            transform.position = Vector3.Lerp(currentPosition, startPosition, Mathf.SmoothStep(0f, 1f, elapsedTime));

            // Ease into the starting rotation
            transform.rotation = Quaternion.Lerp(currentRotation, startRotation, Mathf.SmoothStep(0f, 1f, elapsedTime));

            yield return null;
        }

        // Ensure final position and rotation are set to exact values
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
