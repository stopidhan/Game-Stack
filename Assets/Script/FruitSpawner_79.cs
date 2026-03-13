using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs; // Array untuk menyimpan prefab buah
    public float minX = -8f;          // Batas kiri spawn
    public float maxX = 8f;           // Batas kanan spawn
    public float spawnInterval = 1f;  // Interval waktu antar spawn

    void Start()
    {
        // Mulai memanggil metode SpawnFruit secara periodik
        InvokeRepeating("SpawnFruit", 0f, spawnInterval);
    }

    void SpawnFruit()
    {
        if (fruitPrefabs.Length == 0)
        {
            Debug.LogError("No fruit prefabs assigned!");
            return;
        }

        // Pilih prefab acak dari array
        int randomIndex = Random.Range(0, fruitPrefabs.Length);
        GameObject randomFruit = fruitPrefabs[randomIndex];

        // Tentukan posisi spawn
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);

        // Spawn buah
        Instantiate(randomFruit, spawnPosition, Quaternion.identity);
    }
}
