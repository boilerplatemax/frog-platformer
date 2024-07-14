using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour
{
    public float minDelayTime = 1f; // Minimum time to wait before starting the fade
    public float maxDelayTime = 3f; // Maximum time to wait before starting the fade
    public float minFadeTime = 0.5f; // Minimum time it takes to fade out
    public float maxFadeTime = 2f; // Maximum time it takes to fade out

    private SpriteRenderer spriteRenderer;
    private MeshRenderer meshRenderer;
    private Color originalColor;
    private float delayTime;
    private float fadeTime;

    private void Start()
    {
        // Set the layer to "FragmentEffect" or layer 16
        gameObject.layer = LayerMask.NameToLayer("FragmentEffect"); // or 16;

        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (spriteRenderer == null && meshRenderer == null)
        {
            Debug.LogError("No SpriteRenderer or MeshRenderer found on this GameObject.");
            return;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }

        // Get random delay and fade times within the specified ranges
        delayTime = Random.Range(minDelayTime, maxDelayTime);
        fadeTime = Random.Range(minFadeTime, maxFadeTime);

        StartCoroutine(FadeOut());
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.tag == "Danger")
        {
            Destroy(this.gameObject, 0.1f);
        }
    }

    private IEnumerator FadeOut()
    {
        // Wait for the random delay time
        yield return new WaitForSeconds(delayTime);

        // Fade out over the random fade time
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            else if (meshRenderer != null)
            {
                meshRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            yield return null;
        }

        // Ensure the sprite or mesh is fully transparent
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
        else if (meshRenderer != null)
        {
            meshRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
