using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private PlayerControl playerControl;

    public GameObject enemyGroupPrefab;
    public float spawnInterval = 5.0f;
    public float minY = 2.0f;
    public float maxY = 5.0f;

    private float timer = 0.0f;

    void SpawnEnemyGroup()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, transform.position.z);

        Instantiate(enemyGroupPrefab, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemyGroup();
            timer = 0f;
        }

        transform.position = new Vector3(playerControl.transform.position.x + 30, transform.position.y, transform.position.z);
    }

    private void Start()
    {
        playerControl = FindFirstObjectByType<PlayerControl>();
    }
}