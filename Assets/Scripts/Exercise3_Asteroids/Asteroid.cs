/*
 * Assignment: Asteroids Game - Asteroid Script - PART 2
 * 
 * Objective: Create a functional asteroid script. This script will be responsible for the functionality of the asteroids.
 * this should include initial velocity, angular velocity, and breaking into smaller asteroids when destroyed.
 * Remember, asteroids should only spawn through the AsteroidSpawner script. 
 
* Requirements:
* 1. The asteroid should start with a constant speed but a random angular velocity. Both of these are set in the Rigidbody2D
*       The movement direction of the asteroid should not change. 
*       Hint: All movement for the asteroid should be done via a Rigidbody2D and should be able to be set at Start.
* 2. When the asteroid is destroyed, it should spawn two smaller asteroids if it is not already the smallest size. 
*       Hint: How can you use a function to set the AsteroidSpawner variable from a different script?
* 3. When the astroid hits the player, it should destroy the player. 
*/

using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 350f;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;
    private AsteroidSpawner spawner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(minRotation, maxRotation));
        rb.linearVelocity = transform.up * speed;

        // When it's 2D rb, float is used for angular velocity
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);

        spawner = FindAnyObjectByType<AsteroidSpawner>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Make asteroid interact accordingly based on different collisions
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            QuitGame();
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BreakAsteroid();
        }
    }

    /// <summary>
    /// Exit game.
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /// <summary>
    /// Break asteroid to smaller pieces if applicable.
    /// </summary>
    private void BreakAsteroid()
    {
        switch (size)
        {
            case (AsteroidSize.Large):
                SpawnChildren(AsteroidSize.Medium);
                break;
            case (AsteroidSize.Medium):
                SpawnChildren(AsteroidSize.Small);
                break;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Sets AsteroidSpawner from a different script
    /// </summary>
    public void SetSpawner(AsteroidSpawner asteroidSpawner)
    {
        spawner = asteroidSpawner;
    }

    /// <summary>
    /// Spawn x amount of smaller asteriods
    /// </summary>
    private void SpawnChildren(AsteroidSize childSize, int amount = 2)
    {
        Vector3 spawnPos = transform.position;
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(minRotation, maxRotation));

        for (int i = 0; i < amount; i++)
        {
            
            //spawner.SpawnAsteroid(spawnPos, childSize);



        }
    }
}
