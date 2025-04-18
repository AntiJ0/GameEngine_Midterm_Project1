using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Chase }
    public State currentState = State.Idle;

    public Transform player;
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Front Obstacle Detection")]
    public Transform frontCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (distance < detectionRange)
            currentState = State.Chase;
        else
            currentState = State.Idle;

        if (currentState == State.Chase)
            ChasePlayer();
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        RaycastHit2D hit = Physics2D.Raycast(frontCheck.position, new Vector2(direction.x, 0), 0.5f, groundLayer);

        if (hit.collider != null && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        Vector3 scale = transform.localScale;
        scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}