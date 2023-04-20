using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] 
    public GameObject prefabs;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;
    public GameManager gameManager; // Add a GameManager reference
    
    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    private void Spawn()
    {
        GameObject obstacle = Instantiate(prefabs);
        obstacle.transform.position += transform.position;
        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        obstacleScript.gameManager = gameManager; // Set the GameManager reference for the Obstacle instance
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}