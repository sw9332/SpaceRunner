using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float speed = 10f;                 // 좌측으로 이동하는 속도
    public float verticalSpreadSpeed = 1f;   // 위아래 퍼지는 속도 (y축 움직임 속도)
    public float maxVerticalOffset = 2f;     // 최대 퍼지는 범위

    private Vector3 startPosition;
    private float currentVerticalOffset = 0f;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
        currentVerticalOffset = 0f;
        movingUp = (Random.value > 0.5f);  // 처음 움직임 방향 랜덤 결정 (위 또는 아래)
    }

    void Update()
    {
        // 좌측으로 이동
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 위아래로 점점 퍼지게 이동
        if (movingUp)
        {
            currentVerticalOffset += verticalSpreadSpeed * Time.deltaTime;
            if (currentVerticalOffset > maxVerticalOffset)
            {
                currentVerticalOffset = maxVerticalOffset;
                movingUp = false;
            }
        }
        else
        {
            currentVerticalOffset -= verticalSpreadSpeed * Time.deltaTime;
            if (currentVerticalOffset < -maxVerticalOffset)
            {
                currentVerticalOffset = -maxVerticalOffset;
                movingUp = true;
            }
        }

        // y 위치 조정 (처음 y 위치에 vertical offset 더함)
        transform.position = new Vector3(transform.position.x, startPosition.y + currentVerticalOffset, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 데미지 처리...
            Destroy(gameObject);
        }
    }
}