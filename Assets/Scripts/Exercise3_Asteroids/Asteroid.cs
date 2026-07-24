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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize 
    { 
        Small, 
        Medium,
        Large
    }

    private Dictionary<AsteroidSize, int> asteroidPoints = new()
    {
        { AsteroidSize.Small, 100 },
        { AsteroidSize.Medium, 50 },
        { AsteroidSize.Large, 20 }
    };
    
    private Collider2D asteroidCollider;

    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 360f;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;
    private AsteroidSpawner spawner;

    public static bool canKillPlayer = true;

    [SerializeField] private AudioClip explosionSound;

    private Animator animator;
    private bool isExploding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        asteroidCollider = GetComponent<Collider2D>();

        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(minRotation, maxRotation));
        rb.linearVelocity = transform.up * speed;

        // When it's 2D rb, float is used for angular velocity
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isExploding)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player") && canKillPlayer)
        {
            Destroy(collision.gameObject);
            PlayExplosionSound();
            BreakAsteroid();

            PlayerStats.playerLives -= 1;
            PlayerSpawner.isPlayerDead = true;
            GameManager.Instance.DisplayPlayerLives();

            if (PlayerSpawner.isPlayerDead && PlayerStats.playerLives <= 0)
            {
                GameManager.Instance.QuitGame();
            }
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            PlayExplosionSound();
            BreakAsteroid();
        }
    }

    private void BreakAsteroid()
    {
        if (isExploding)
        {
            return;
        }

        isExploding = true;

        StartCoroutine(ExplosionSequence());
    }

    /// <summary>
    /// Play explosion sound.
    /// </summary> 
    private void PlayExplosionSound()
    {
        // Creates new game object because when the asteroid is destroyed, sound will not play
        GameObject soundObject = new GameObject("Explosion Sound");
        AudioSource source = soundObject.AddComponent<AudioSource>();

        source.clip = explosionSound;
        source.Play();

        Destroy(soundObject, explosionSound.length);
    }

    private IEnumerator ExplosionSequence()
    {
        // Stop movement
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Disable collisions during explosion
        asteroidCollider.enabled = false;

        // Play explosion animation
        animator.SetTrigger("Explode");

        // Wait for animation
        float explosionAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(explosionAnimationLength);

        // Destroy parent asteroid
        Destroy(gameObject);

        GameManager.Instance.AddPoints(asteroidPoints[size]);

        // spawn children
        if (size > AsteroidSize.Small)
        {
            SpawnChildren(size - 1);
        }
    }

    /// <summary>
    /// Assigns the AsteroidSpawner reference for this asteroid instance.
    /// </summary>
    /// <param name="asteroidSpawner">spawner responsible for creating this asteroid</param>
    public void SetSpawner(AsteroidSpawner asteroidSpawner)
    {
        spawner = asteroidSpawner;
    }

    /// <summary>
    /// Spawn x amount of smaller asteriods
    /// </summary>
    /// <param name="childSize">children asteroid size</param> 
    /// <param name="amount">number of children asteroids to spawn</param>
    private void SpawnChildren(AsteroidSize childSize, int amount = 2)
    {
        Vector3 spawnPos = transform.position;
        float angleOffset;
        float angleFactor = 10;

        for (int i = 0; i < amount; i++)
        {
            angleOffset = Random.Range(minRotation/angleFactor, maxRotation/angleFactor);
            spawner.SpawnAsteroid(spawnPos, childSize, angleOffset);
        }
    }
}
