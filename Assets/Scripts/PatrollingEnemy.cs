using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public bool usePredefinedRange = false;
    public LayerMask patrolLayers;
    public Vector2 patrolRangeX = new Vector2(-2, 3);
    public Vector2 patrolRangeY = new Vector2(-2, 3);
    public bool patrolHorizontally = true; // true for horizontal, false for vertical

    public enum StartingDirection { Left, Right, Up, Down }
    public StartingDirection startDirection = StartingDirection.Right;

    private Vector2 startPosition;
    private bool movingPositiveDirection;

    void Start()
    {
        startPosition = transform.position;
        InitializeDirection();
    }

    void Update()
    {
        Move();

        if (usePredefinedRange)
        {
            PatrolPredefinedRange();
        }
    }

    private void InitializeDirection()
    {
        switch (startDirection)
        {
            case StartingDirection.Left:
                movingPositiveDirection = false;
                break;
            case StartingDirection.Right:
                movingPositiveDirection = true;
                break;
            case StartingDirection.Up:
                movingPositiveDirection = true;
                break;
            case StartingDirection.Down:
                movingPositiveDirection = false;
                break;
        }
    }

    private void Move()
    {
        Vector2 direction = Vector2.zero;

        if (patrolHorizontally)
        {
            direction = movingPositiveDirection ? Vector2.right : Vector2.left;
            GetComponent<SpriteRenderer>().flipX = !movingPositiveDirection;
        }
        else
        {
            direction = movingPositiveDirection ? Vector2.up : Vector2.down;
        }

        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void PatrolPredefinedRange()
    {
        if (patrolHorizontally)
        {
            if (movingPositiveDirection && transform.position.x >= startPosition.x + patrolRangeX.y)
            {
                movingPositiveDirection = false;
            }
            else if (!movingPositiveDirection && transform.position.x <= startPosition.x + patrolRangeX.x)
            {
                movingPositiveDirection = true;
            }
        }
        else
        {
            if (movingPositiveDirection && transform.position.y >= startPosition.y + patrolRangeY.y)
            {
                movingPositiveDirection = false;
            }
            else if (!movingPositiveDirection && transform.position.y <= startPosition.y + patrolRangeY.x)
            {
                movingPositiveDirection = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!usePredefinedRange && ((1 << collision.gameObject.layer) & patrolLayers) != 0)
        {
            movingPositiveDirection = !movingPositiveDirection;
        }
    }
}
