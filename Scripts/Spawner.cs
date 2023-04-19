using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] 
    public GameObject prefabs;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;
    
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
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}