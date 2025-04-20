using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5;
    public float lifetime = 5f;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.left * speed; // ← 왼쪽으로 발사 (원하는 방향으로 수정)
        }
        else
        {
            Debug.LogError("Rigidbody2D가 없습니다!");
        }

        Destroy(gameObject, lifetime);
    }
}
