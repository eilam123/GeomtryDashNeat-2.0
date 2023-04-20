using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ground groundPrefab;
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;

    public float gameSpeed { get; private set; }
    public Player player; // Change this line to make it a public variable
    public Spawner spawner; // Change this line to make it a public variable
    private Ground ground;
    private float score;

    public void Initialize()
    {
        Ground groundInstance = Instantiate(groundPrefab);
        ground = groundInstance.GetComponent<Ground>();
        ground.gameManager = this;

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        ground.enabled = true;
        gameSpeed = initialGameSpeed;
        score = 0f;
        enabled = true;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
    }
    public float UpdateGame()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        return score;
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        ground.enabled = false;
    }
}