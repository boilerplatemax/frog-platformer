using UnityEngine;
using System.Collections;
using TarodevController;
using TarodevController.Demo;
using UnityEngine.Tilemaps;

public class GrappleVisualizer : MonoBehaviour
{
    private PlayerController playerController;
    public Transform firePoint;
    public GameObject selectorSprite;
    public GameObject grapplePointer;

    public float pointerRadius;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure z-coordinate is zero
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // Calculate the position of the grapplePointer
        Vector2 grapplePosition = firePoint.position + (Vector3)(direction * pointerRadius);
        grapplePointer.transform.position = grapplePosition;

        // Rotate the grapplePointer to face outward
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        grapplePointer.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, Mathf.Infinity, playerController.grappleLayer);

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
                if (distance > playerController.maxGrappleLength)
                {
                    SetSpriteOpacity(selectorSprite, 0.25f);
                }
                else
                {
                    SetSpriteOpacity(selectorSprite, 1.0f);
                }
            }
            else
            {
                selectorSprite.SetActive(false);
            }
        }
        else
        {
            selectorSprite.SetActive(false);
        }
    }

    private Vector2 AdjustToNearestHalf(Vector2 hitPoint, Vector3 firePoint)
    {
        float adjustedX = hitPoint.x;
        float adjustedY = hitPoint.y;

        // Adjust X based on the direction
        if (firePoint.x <= hitPoint.x)
        {
            // Fire point is to the left of hit point, round up to nearest 0.5
            adjustedX = Mathf.Floor(hitPoint.x) + 0.5f;
            if (adjustedX <= hitPoint.x) adjustedX += 1.0f;
        }
        else
        {
            // Fire point is to the right of hit point, round down to nearest 0.5
            adjustedX = Mathf.Floor(hitPoint.x) + 0.5f;
            if (adjustedX > hitPoint.x) adjustedX -= 1.0f;
        }

        // Adjust Y based on the direction
        if (firePoint.y <= hitPoint.y)
        {
            // Fire point is below the hit point, round up to nearest 0.5
            adjustedY = Mathf.Floor(hitPoint.y) + 0.5f;
            if (adjustedY <= hitPoint.y) adjustedY += 1.0f;
        }
        else
        {
            // Fire point is above the hit point, round down to nearest 0.5
            adjustedY = Mathf.Floor(hitPoint.y) + 0.5f;
            if (adjustedY > hitPoint.y) adjustedY -= 1.0f;
        }

        return new Vector2(adjustedX, adjustedY);
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
