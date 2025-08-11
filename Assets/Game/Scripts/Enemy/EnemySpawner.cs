using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 3f;

    private Transform player;

    private void Start()
    {
        player = PlayerController.Instance.transform;
        // InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        SpawnEnemy(); // Initial spawn
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = player.position + (Random.insideUnitSphere * spawnRadius);
        spawnPos.y = 1f; // Keep on ground level

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
