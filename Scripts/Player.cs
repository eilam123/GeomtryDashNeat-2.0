using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEAT;
public class Player : MonoBehaviour
{
    public GameManager gameManager { get; set; }
    private CharacterController character;
    private Vector3 direction;
    private bool isJumping = false;
    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;
    public float turnAngle = 90f;
    public float rotationDuration = 1f; // Duration for completing the rotation

    private float rotationTimer = 0f; // Timer for tracking rotation progress
    private Quaternion initialRotation; // Initial rotation of the object

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        
    }

    /*void Update()
    {
        if (character.isGrounded)
        {
            direction = Vector3.down;
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                rotationTimer = 0f; // Reset rotation timer
                initialRotation = transform.rotation; // Store initial rotation
                direction += Vector3.up * jumpForce;
            }
        }
        else
        {
            if (isJumping)
            {
                rotationTimer += Time.deltaTime;
                if (rotationTimer <= rotationDuration)
                {
                    // Rotate the object clockwise around the Z-axis
                    float rotationAmount = (turnAngle / rotationDuration) * Time.deltaTime;
                    transform.Rotate(Vector3.forward, rotationAmount);
                }
                else
                {
                    // Reset rotation timer and flag
                    rotationTimer = 0f;
                    isJumping = false;
                }
            }
            direction += Vector3.down * gravity * Time.deltaTime;
        }

        character.Move(direction * Time.deltaTime);
    }*/
    public void UpdateClient(Client client)
    {
        if (character.isGrounded)
        {
            direction = Vector3.down;
            double[] inputs = { gameManager.gameSpeed, GetClosestObstacleDistance(), transform.position.y };
            double[] output = client.Calculate(inputs);

            if (output[0] > 0.5)
            {
                isJumping = true;
                rotationTimer = 0f; // Reset rotation timer
                initialRotation = transform.rotation; // Store initial rotation
                direction += Vector3.up * jumpForce;
            }
        }
        // ... rest of the method remains unchanged
        else
        {
            if (isJumping)
            {
                rotationTimer += Time.deltaTime;
                if (rotationTimer <= rotationDuration)
                {
                    // Rotate the object clockwise around the Z-axis
                    float rotationAmount = (turnAngle / rotationDuration) * Time.deltaTime;
                    transform.Rotate(Vector3.forward, rotationAmount);
                }
                else
                {
                    // Reset rotation timer and flag
                    rotationTimer = 0f;
                    isJumping = false;
                }
            }
            direction += Vector3.down * gravity * Time.deltaTime;
        }

        character.Move(direction * Time.deltaTime);
    }
    private float GetClosestObstacleDistance()
    {
        // Replace "Obstacle" with the tag you use for your obstacles
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        float minDistance = Mathf.Infinity;

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);
            minDistance = Mathf.Min(minDistance, distance);
        }

        return minDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }
}