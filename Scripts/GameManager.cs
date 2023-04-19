using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    private Player player;
    private Spawner spawner;
    private Ground ground;
	public TextMeshProUGUI gameOverText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI hiscoreText;
	public Button retryButton;
	private float score;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestory()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        ground = FindObjectOfType<Ground>();
        NewGame();
    }

    public void NewGame()
    {
        //Obstacle obstacle = FindObjectOfType<Obstacle>();
        //Destroy(obstacle);
 		Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }
        ground.enabled = true;
        gameSpeed = initialGameSpeed;
		score = 0f;
        enabled = true;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
		gameOverText.gameObject.SetActive(false);
		retryButton.gameObject.SetActive(false);
        
        UpdateHiscore();

    }
    
    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
	    score += gameSpeed * Time.deltaTime;
		scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }
    
    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        ground.enabled = false;
		gameOverText.gameObject.SetActive(true);
		retryButton.gameObject.SetActive(true);
	    UpdateHiscore();
    }
	
	private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
