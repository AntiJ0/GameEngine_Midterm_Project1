using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform player;
    public float tileWidth = 20f;

    private Transform[] tiles = new Transform[2];
    private float lastPlayerX;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // ✅ 자식 배경 오브젝트 가져오기
        tiles[0] = transform.GetChild(0); // 첫 배경

        // ✅ 복제 시 부모는 복제 안 하고, 자식만 복제
        GameObject clone = Instantiate(tiles[0].gameObject, tiles[0].position + Vector3.right * tileWidth, Quaternion.identity);
        tiles[1] = clone.transform;

        lastPlayerX = player.position.x;
    }

    void Update()
    {
        float deltaX = player.position.x - lastPlayerX;

        if (Mathf.Abs(deltaX) >= tileWidth / 2f)
        {
            Transform furthest = tiles[0].position.x < tiles[1].position.x ? tiles[0] : tiles[1];
            Transform closest = tiles[0].position.x < tiles[1].position.x ? tiles[1] : tiles[0];

            if (deltaX > 0)
                furthest.position = new Vector3(closest.position.x + tileWidth, furthest.position.y, furthest.position.z);
            else
                furthest.position = new Vector3(closest.position.x - tileWidth, furthest.position.y, furthest.position.z);

            lastPlayerX = player.position.x;
        }
    }
}