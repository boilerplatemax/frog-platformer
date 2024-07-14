using UnityEngine;
using System.Collections;

public class GrappleObjectHandler : MonoBehaviour
{
    public Vector2 grappleOffset = Vector2.zero;
    public float regrappleTime = 1.0f;
    public Animator animator;

    private bool canBeGrappled = true;

    public bool CanBeGrappled
    {
        get { return canBeGrappled; }
    }

    public void TriggerGrappleAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Grapple");
        }
    }

    public void StartRegrappleCooldown()
    {
        StartCoroutine(RegrappleCooldown());
    }

    private IEnumerator RegrappleCooldown()
    {
        canBeGrappled = false;
        yield return new WaitForSeconds(regrappleTime);
        canBeGrappled = true;
    }
}
