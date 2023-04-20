using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEAT;

public class NEAT_Trainer : MonoBehaviour
{
    public int numClients = 10;
    public float gameTime = 10f;
    private Neat neat;
    public GameManager gameManagerPrefab; // Add this line to store a reference to the GameManager prefab


    void Start()
    {
        // Initialize NEAT with the given number of clients
        neat = new Neat(3, 1, numClients);

        // Train clients simultaneously
        StartCoroutine(TrainClients());
    }

    IEnumerator TrainClients()
    {
        while (true)
        {
            // Run the game simulation for each client
            for (int i = 0; i < neat.GetMaxClients(); i++)
            {
                Client client = neat.GetClient(i);

                // Create a new instance of your game
                GameManager gameManager = Instantiate(gameManagerPrefab);
                Player player = gameManager.player;
                Spawner spawner = gameManager.spawner;
                spawner.gameManager = gameManager; // Set the GameManager reference for the Spawner instance
                player.gameManager = gameManager;

                // Run the game and evaluate the client's performance
                float score = RunGame(client, player, gameManager);
                client.SetScore(score);

                // Clean up the game instance
                Destroy(gameManager.gameObject);
            }

            // Evolve the clients based on their performance
            neat.Evolve();

            // Wait for some time before starting the next generation
            yield return new WaitForSeconds(gameTime);
        }
    }

    float RunGame(Client client, Player player, GameManager gameManager)
    {
        gameManager.Initialize();
        float startTime = Time.time;
        float endTime = startTime + gameTime;
        float score = 0f;

        while (Time.time < endTime)
        {
            // Set the gameManager reference for all spawned obstacles
            Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
            foreach (Obstacle obstacle in obstacles)
            {
                if (obstacle.gameManager == null)
                {
                    obstacle.gameManager = gameManager;
                }
            }

            player.UpdateClient(client);
            score = gameManager.UpdateGame();

            // Check if the game is over
            if (!player.gameObject.activeSelf)
            {
                break;
            }
        }

        gameManager.GameOver();
        return score;
    }
    
}