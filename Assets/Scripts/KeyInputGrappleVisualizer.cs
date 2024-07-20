using UnityEngine;
using TarodevController;
using TarodevController.Demo;
using UnityEngine.Tilemaps;

public class KeyInputGrappleVisualizer : MonoBehaviour
{
    private PlayerController playerController;
    public Transform firePoint;
    public GameObject selectorSprite;
    public GameObject grapplePointer;

    public float pointerRadius;
    public Material hitMaterial; // Public material for hit
    public Material regularMaterial; // Public material for regular

    private SpriteRenderer selectorSpriteRenderer;
    private SpriteRenderer grapplePointerSpriteRenderer;
    private GameObject lastHitObject = null;
    private Material lastOriginalMaterial = null;
    private SpriteRenderer lastHitRenderer = null;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        selectorSpriteRenderer = selectorSprite.GetComponent<SpriteRenderer>();
        grapplePointerSpriteRenderer = grapplePointer.GetComponent<SpriteRenderer>();

        if (playerController != null)
        {
            Debug.Log("PlayerController component found.");
        }
        else
        {
            Debug.LogError("PlayerController component not found.");
        }
    }

    private void Update()
    {
        if (playerController == null) return;

        Vector2 direction;
        bool hasInput = false;

        if (playerController.useKeyInputForGrapple)
        {
            direction = new Vector2(playerController._frameInput.Move.x, playerController._frameInput.Move.y).normalized;
            hasInput = direction != Vector2.zero;
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure z-coordinate is zero
            direction = (mousePosition - firePoint.position).normalized;
            hasInput = Input.GetMouseButton(0);
        }

        grapplePointer.SetActive(hasInput);

        if (hasInput)
        {
            // Calculate the position of the grapplePointer
            Vector2 grapplePosition = firePoint.position + (Vector3)(direction * pointerRadius);
            grapplePointer.transform.position = grapplePosition;

            // Rotate the grapplePointer to face outward
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            grapplePointer.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        // Check if grappling is possible
        bool canGrapple = playerController.canGrapple;

        // Set the opacity based on canGrapple
        SetGrapplePointerOpacity(canGrapple);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, playerController.maxGrappleLength, playerController.grappleLayer);

        if (hit.collider != null && (playerController.ignoreLayer.value & (1 << hit.collider.gameObject.layer)) == 0)
        {
            // Check if the hit collider is a TilemapCollider2D or CompositeCollider2D
            if (hit.collider is TilemapCollider2D || hit.collider is CompositeCollider2D)
            {
                Vector2 hitPoint = hit.point;
                Vector2 adjustedPoint = AdjustToNearestHalf(hitPoint, firePoint.position);
                selectorSprite.transform.position = adjustedPoint;
                selectorSprite.SetActive(true);

                float distance = Vector2.Distance(firePoint.position, hitPoint);
                SetSpriteOpacity(selectorSprite, distance > playerController.maxGrappleLength ? 0.25f : 1.0f);

                ResetLastHitObject();
            }
            else
            {
                // Change the material of the hit object if it is not a TilemapCollider2D or CompositeCollider2D and player can grapple
                SpriteRenderer hitRenderer = hit.collider.GetComponent<SpriteRenderer>();
                if (hitRenderer != null && canGrapple)
                {
                    // If this is a new hit object, reset the previous one
                    if (hit.collider.gameObject != lastHitObject)
                    {
                        ResetLastHitObject();

                        // Store the current hit object and its original material
                        lastHitObject = hit.collider.gameObject;
                        lastOriginalMaterial = hitRenderer.material;

                        // Set the hit material
                        hitRenderer.material = hitMaterial;
                    }
                }
                else if (hitRenderer != null && hitRenderer.material != regularMaterial)
                {
                    // If the player cannot grapple, ensure the material is reset
                    hitRenderer.material = regularMaterial;
                }

                selectorSprite.SetActive(false);
            }
        }
        else
        {
            ResetLastHitObject();
            selectorSprite.SetActive(false);
        }
    }

    private void SetGrapplePointerOpacity(bool canGrapple)
    {
        if (grapplePointerSpriteRenderer != null)
        {
            Color color = grapplePointerSpriteRenderer.color;
            color.a = canGrapple ? 1.0f : 0.5f;
            grapplePointerSpriteRenderer.color = color;
        }
    }

    private void ResetLastHitObject()
    {
        if (lastHitObject != null)
        {
            if (lastHitRenderer == null)
            {
                lastHitRenderer = lastHitObject.GetComponent<SpriteRenderer>();
            }
            if (lastHitRenderer != null)
            {
                lastHitRenderer.material = lastOriginalMaterial;
            }

            lastHitObject = null;
            lastOriginalMaterial = null;
            lastHitRenderer = null;
        }
    }

    private Vector2 AdjustToNearestHalf(Vector2 hitPoint, Vector3 firePoint)
    {
        float adjustedX = firePoint.x <= hitPoint.x ? Mathf.Ceil(hitPoint.x) : Mathf.Floor(hitPoint.x);
        float adjustedY = firePoint.y <= hitPoint.y ? Mathf.Ceil(hitPoint.y) : Mathf.Floor(hitPoint.y);
        return new Vector2(adjustedX + 0.5f, adjustedY + 0.5f);
    }

    private void SetSpriteOpacity(GameObject sprite, float opacity)
    {
        SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = opacity;
            renderer.color = color;
        }
    }
}
