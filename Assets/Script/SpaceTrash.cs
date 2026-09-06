using UnityEngine;

public class SpaceTrash : MonoBehaviour
{
    public float attractSpeed = 100f; // 의미 없어질 수 있음
    private PlayerControl Player;
    private bool isAttracting = false;
    private Rigidbody rb;

    void Start()
    {
        Player = FindFirstObjectByType<PlayerControl>();
        rb = GetComponent<Rigidbody>();

        // Rigidbody 기본 세팅 강제 적용 (필수)
        if (rb != null)
        {
            rb.useGravity = false;
            rb.drag = 0f;
            rb.mass = 1f;
            rb.constraints = RigidbodyConstraints.FreezeRotation; // 회전 막기
        }
    }

    void Update()
    {
        if (isAttracting && Player != null)
        {
            // 플레이어 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 50f * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, Player.transform.position);
            if (distance < 1.5f) // 수거 판정 거리
            {
                // 수거 처리
                SpaceTrashSpawner spawner = FindFirstObjectByType<SpaceTrashSpawner>();
                if (spawner != null)
                {
                    SpaceTrashSpawner.SpaceTrash++;
                    spawner.SpaceTrashText.text = "Space Trash : " + SpaceTrashSpawner.SpaceTrash.ToString();
                }

                Destroy(gameObject); // 자폭
            }
        }
    }

    public void StartAttract()
    {
        isAttracting = true;
    }

    public void StopAttract()
    {
        isAttracting = false;
    }
}