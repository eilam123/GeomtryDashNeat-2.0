using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameManager gameManager { get; set; }
    private float leftEdge;
    
    // Start is called before the first frame update
    void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero).x - 2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * gameManager.gameSpeed * Time.deltaTime;
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }

}
