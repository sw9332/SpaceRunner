using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SpaceTrashSpawner : MonoBehaviour
{
    [Header("스폰할 우주 쓰레기 프리팹")]
    public GameObject[] spaceTrashPrefab;

    [Header("스폰 개수 (한 번에 스폰할 개수)")]
    public int trashCountPerSpawn = 5;

    [Header("스폰 범위 크기")]
    public Vector3 spawnSize = new Vector3(50f, 20f, 100f);

    [Header("스폰 간격 (초)")]
    public float spawnInterval = 3f;

    private PlayerControl playerControl;

    private float spawnTimer = 0f;

    public Text CleanerText;

    public static int SpaceTrash;
    public Text SpaceTrashText;

    void Start()
    {
        playerControl = FindFirstObjectByType<PlayerControl>();
        
        transform.position = playerControl.transform.position;

        CleanerText.text = "";
        SpaceTrashText.text = "Space Trash : "+SpaceTrash; // 초기 텍스트 설정
    }

    void Update()
    {
        // 스포너 위치를 플레이어 위치로 이동
        transform.position = new Vector3(playerControl.transform.position.x + 60, playerControl.transform.position.y, 0);

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnTrash();
            spawnTimer = 0f;
        }

        if (playerControl.isAttractActive)
        {
            CleanerText.text = "청소 중...";
        }
        else
        {
            CleanerText.text = "";
        }
    }

    void SpawnTrash()
    {
        for (int i = 0; i < trashCountPerSpawn; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
                Random.Range(-spawnSize.y / 2, spawnSize.y / 2),
                Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
            );

            int randomIndex = Random.Range(0, spaceTrashPrefab.Length);
            Instantiate(spaceTrashPrefab[randomIndex], randomPos, Quaternion.identity);

            SpaceTrash++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        // 플레이어 위치가 아닌 스포너 위치 기준으로 그리기
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }
}