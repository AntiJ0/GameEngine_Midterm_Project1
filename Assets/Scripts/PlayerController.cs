using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool canMove = true;

    public float moveSpeed = 5f;
    private float originalSpeed;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;

    private bool isInvincible = false;
    private bool isTouchingTrap = false;

    private bool isInvincibleActive = false;
    private bool isSpeedBoostActive = false;
    private bool isJumpBoostActive = false;

    private Color originalColor;
    private float originalJumpForce;

    private SpriteRenderer spriteRenderer;
    private Coroutine invincibilityRoutine;
    private Coroutine speedBoostRoutine;
    private Coroutine jumpBoostRoutine;

    // ▼ 추가된 부분 (투사체 관련)
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 0.5f;
    private bool canAttack = false;
    private float lastFireTime;

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
            if (!isInvincible)
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        if (collision.CompareTag("Invincibility Item"))
        {
            ActivateInvincibility(5f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Speed Boost Item"))
        {
            BoostSpeed(1.5f, 10f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Jump Boost Item"))
        {
            BoostJump(1.4f, 10f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("CageTrigger"))
        {
            GameObject cageTilemap = GameObject.FindGameObjectWithTag("EnemyCage");
            if (cageTilemap != null)
            {
                cageTilemap.SetActive(false);
            }
        }

        if (collision.CompareTag("Boss"))
        {
            SceneManager.LoadScene("GameOver");
        }

        if (collision.CompareTag("Attack Item")) // ⭐ 공격 아이템 처리 추가
        {
            canAttack = true;
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

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                SceneManager.LoadScene("GameOver");
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
        isInvincibleActive = true;

        UpdatePlayerColor();

        float warningTime = 2f;
        float blinkStartTime = Time.time + (duration - warningTime);

        while (Time.time < blinkStartTime)
            yield return null;

        float endTime = Time.time + warningTime;
        float blinkSpeed = 0.2f;

        while (Time.time < endTime)
        {
            isInvincibleActive = false;
            UpdatePlayerColor();
            yield return new WaitForSeconds(blinkSpeed);

            isInvincibleActive = true;
            UpdatePlayerColor();
            yield return new WaitForSeconds(blinkSpeed);

            blinkSpeed *= 0.8f;
        }

        isInvincible = false;
        isInvincibleActive = false;
        UpdatePlayerColor();
    }

    public void BoostSpeed(float multiplier, float duration)
    {
        if (speedBoostRoutine != null)
        {
            StopCoroutine(speedBoostRoutine);
            moveSpeed = originalSpeed;
        }

        speedBoostRoutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        moveSpeed *= multiplier;
        isSpeedBoostActive = true;
        UpdatePlayerColor();

        float warningTime = 3f;
        float blinkStartTime = Time.time + (duration - warningTime);

        while (Time.time < blinkStartTime)
            yield return null;

        float endTime = Time.time + warningTime;
        float blinkSpeed = 0.3f;

        while (Time.time < endTime)
        {
            isSpeedBoostActive = false;
            UpdatePlayerColor();
            yield return new WaitForSeconds(blinkSpeed);

            isSpeedBoostActive = true;
            UpdatePlayerColor();
            yield return new WaitForSeconds(blinkSpeed);

            blinkSpeed *= 0.82f;
        }

        isSpeedBoostActive = false;
        moveSpeed = originalSpeed;
        UpdatePlayerColor();
    }

    public void BoostJump(float multiplier, float duration)
    {
        if (jumpBoostRoutine != null)
        {
            StopCoroutine(jumpBoostRoutine);
            jumpForce = originalJumpForce;
        }

        jumpBoostRoutine = StartCoroutine(JumpBoostCoroutine(multiplier, duration));
    }

    private IEnumerator JumpBoostCoroutine(float multiplier, float duration)
    {
        jumpForce *= multiplier;
        isJumpBoostActive = true;
        UpdatePlayerColor();

        float warningTime = 3f;
        float blinkStartTime = Time.time + (duration - warningTime);

        while (Time.time < blinkStartTime)
            yield return null;

        float endTime = Time.time + warningTime;
        float blinkSpeed = 0.3f;

        while (Time.time < endTime)
        {
            isJumpBoostActive = false;
            UpdatePlayerColor();
            yield return new WaitForSeconds(blinkSpeed);

            isJumpBoostActive = true;
            UpdatePlayerColor();
            yield return new WaitForSeconds(blinkSpeed);

            blinkSpeed *= 0.82f;
        }

        isJumpBoostActive = false;
        jumpForce = originalJumpForce;
        UpdatePlayerColor();
    }

    private void UpdatePlayerColor()
    {
        Color finalColor = originalColor;

        bool anyEffect = isInvincibleActive || isSpeedBoostActive || isJumpBoostActive;

        if (!anyEffect)
        {
            spriteRenderer.color = finalColor;
            return;
        }

        float alpha = isInvincibleActive ? 0.5f : 1f;

        float r = 0f, g = 0f, b = 0f;
        int count = 0;

        if (isSpeedBoostActive)
        {
            r += 0.5f; g += 1f; b += 0.5f; count++;
        }

        if (isJumpBoostActive)
        {
            r += 0.5f; g += 0.85f; b += 1f; count++;
        }

        if (count > 0)
        {
            r /= count; g /= count; b /= count;
            finalColor = new Color(r, g, b, alpha);
        }
        else if (isInvincibleActive)
        {
            finalColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        spriteRenderer.color = finalColor;
    }

    public void EnableAttack() => canAttack = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;
        originalJumpForce = jumpForce;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero; // ✅ 이동 완전 정지
            return;
        }

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

        if (!isInvincible && isTouchingTrap)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (canAttack && Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireCooldown)
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            lastFireTime = Time.time;
        }
    }
}