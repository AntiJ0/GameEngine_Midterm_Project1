using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Return }
    public State currentState = State.Patrol;

    [Header("References")]
    public Transform player;
    public Transform groundCheck;
    public Transform wallCheck;

    [Header("Settings")]
    public float speed = 2f;
    public float chaseRange = 5f;
    public float returnThreshold = 8f;
    public float jumpForce = 7f;
    public float wallCheckDistance = 0.3f;

    [Header("Layers")]
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 originalPosition;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (distanceToPlayer <= chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase:
                Chase();
                if (distanceToPlayer > returnThreshold)
                    currentState = State.Return;
                break;

            case State.Return:
                ReturnToOrigin();
                if (Vector2.Distance(transform.position, originalPosition) < 0.2f)
                    currentState = State.Patrol;
                break;
        }
    }

    void Patrol()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        // Boundary 감지 → 무조건 방향 전환
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, direction, wallCheckDistance);
        if (wallHit.collider != null && wallHit.collider.CompareTag("Boundary"))
        {
            Flip();
            return; // 여기서 멈추면 더 이상 이동 X
        }

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    void Chase()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (IsBoundaryAhead() && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        if (direction.x > 0 && !facingRight)
            Flip();
        else if (direction.x < 0 && facingRight)
            Flip();
    }

    void ReturnToOrigin()
    {
        Vector2 direction = (originalPosition - (Vector2)transform.position).normalized;

        if (IsBoundaryAhead() && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        if (direction.x > 0 && !facingRight)
            Flip();
        else if (direction.x < 0 && facingRight)
            Flip();
    }

    bool IsBoundaryAhead()
    {
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance);
        return hit.collider != null && hit.collider.CompareTag("Boundary");
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Vector3 dir = facingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + dir * wallCheckDistance);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }
}