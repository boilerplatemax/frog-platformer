using UnityEngine;
using TarodevController;
using System.Collections;

public class PlatformEffectorToggle : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    private PlatformEffector2D platformEffector;
    private BoxCollider2D boxCollider;

    private bool isPressingDown;
    private bool isOnWall;
    private float colliderDisableDuration = 0.25f;

    void Start()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
    }

    void Update()
    {
        if (playerController == null) return;

        isPressingDown = playerController.Input.y == -1;
        isOnWall = playerController._isOnWall;

        if (isOnWall)
        {
            SetPlatformEffectorState(false, false);
        }
        else if (isPressingDown)
        {
            StartCoroutine(TemporaryDisableCollider());
        }
        else
        {
            SetPlatformEffectorState(true, true);
        }
    }

    private IEnumerator TemporaryDisableCollider()
    {
        SetPlatformEffectorState(false, false);
        yield return new WaitForSeconds(colliderDisableDuration);
        SetPlatformEffectorState(true, true);
    }

    private void SetPlatformEffectorState(bool effectorEnabled, bool colliderEnabled)
    {
        if (platformEffector.enabled != effectorEnabled)
            platformEffector.enabled = effectorEnabled;

        if (boxCollider.enabled != colliderEnabled)
            boxCollider.enabled = colliderEnabled;
    }

    // Expose colliderDisableDuration to Unity Editor
    public float ColliderDisableDuration
    {
        get { return colliderDisableDuration; }
        set { colliderDisableDuration = value; }
    }
}
