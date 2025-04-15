using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;

    public bool isInvincible = false;
    private bool isTouchingTrap = false;

    private SpriteRenderer spriteRenderer;
    private Coroutine invincibilityRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Game Over"))
        {
            SceneManager.LoadScene("GameOver");
        }

        if (collision.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("GameOver");
        }

        if (collision.CompareTag("Invincibility Item"))
        {
            ActivateInvincibility(5f);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            isTouchingTrap = true;

            if (!isInvincible)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            isTouchingTrap = false;
        }
    }

    public void ActivateInvincibility(float duration)
    {
        if (invincibilityRoutine != null)
            StopCoroutine(invincibilityRoutine);

        invincibilityRoutine = StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        // 1단계: 처음엔 반투명 상태 유지
        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        spriteRenderer.color = transparentColor;

        float warningTime = 2f; // 깜빡이기 시작할 시점 (마지막 2초)
        float blinkStartTime = Time.time + (duration - warningTime);

        while (Time.time < blinkStartTime)
        {
            yield return null;
        }

        // 2단계: 마지막 2초 동안 깜빡임 (점점 빨라지게)
        float blinkDuration = duration - (Time.time - (blinkStartTime - (duration - warningTime)));
        float endTime = Time.time + blinkDuration;
        float blinkSpeed = 0.2f;

        while (Time.time < endTime)
        {
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);

            spriteRenderer.color = transparentColor;
            yield return new WaitForSeconds(blinkSpeed);

            blinkSpeed *= 0.85f; // 점점 빨라짐
        }

        // 3단계: 무적 끝나고 원래 상태 복구
        spriteRenderer.color = originalColor;
        isInvincible = false;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        if (moveInput > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("JumpAction");
            pAni.SetTrigger("FallAction");
        }

        // 무적이 꺼졌는데 현재 함정과 닿아있다면 게임오버
        if (!isInvincible && isTouchingTrap)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
