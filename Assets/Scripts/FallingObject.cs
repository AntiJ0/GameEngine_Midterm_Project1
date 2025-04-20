using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // 일정 시간 후 자동 제거 (안 떨어졌을 경우 대비)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ground 레이어에 닿으면 파괴
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}