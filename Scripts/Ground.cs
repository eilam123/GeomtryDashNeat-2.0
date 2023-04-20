using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public GameManager gameManager { get; set; }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gameManager != null)
        {
            float speed = gameManager.gameSpeed / transform.localScale.x;
            meshRenderer.material.mainTextureOffset += Vector2.right * Time.deltaTime;
        }
    }
}