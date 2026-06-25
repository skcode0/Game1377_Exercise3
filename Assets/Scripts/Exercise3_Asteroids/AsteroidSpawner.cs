/*
 * Assignment: Asteroids Game - AstroidSpawner Script - PART 2
 * 
 * Objective: Create a functional asteroid spawning script. This script will be responsible for spawning
 * asteroids at the start of the game, as well as spawning smaller asteroids when larger asteroids are destroyed. 
 * ALL ASTEROID SPAWNING SHOULD OCCUR THROUGH THIS SCRIPT. 
 
* Requirements:
* 1. Fill in the SpawnAsteroids method to spawn an asteroid at a location specified by the position and size parameters.
*       Hint: You may need to create a variable for the prefabs you need. 
*       Hint: Use the spawnXMax, spawnXMin, spawnYMax, and spawnYMin variables to determine where the asteroids can spawn.
* 2. Spawn a variable number of asteroids at the start of the game using the SpawnInitialAsteroids() method.
*       This should be determined by a private variable that can be set in the editor (set it to 5 in the Inspector). 
*       The asteroids should spawn at random positions within the camera view, but not too close to the center (0,0)
*       where the player will be (at least 3 units away from the center in any direction).
*       Hint: Vector3.Distance can tell you how far one point is away from another. 
*/
using System.Drawing;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid largeAsteroid;
    [SerializeField] private Asteroid mediumAsteroid;
    [SerializeField] private Asteroid smallAsteroid;

    [SerializeField] private int initialAsteroidCount = 5;

    // These variables determine the spawn area for the asteroids.
    // They are calculated at Start based off of the camera size. 
    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;
    private float playerSafeDistance = 3f;

    void Start()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
        spawnXMax = screenHalfWidth + playerSafeDistance;
        spawnXMin = -screenHalfWidth - playerSafeDistance;
        spawnYMax = screenHalfHeight + playerSafeDistance;
        spawnYMin = -screenHalfHeight - playerSafeDistance;
        SpawnInitialAsteroids(initialAsteroidCount);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Spawn initial asteroids at random positions within boundaries but also where there's no player.
    /// </summary>
    private void SpawnInitialAsteroids(int amount=5)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = GetValidSpawnPos();

            SpawnAsteroid(spawnPos, Asteroid.AsteroidSize.Large);
        }
    }

    /// <summary>
    /// Spawn an asteroid at the location specified by position parameter with the size specified by the size parameter.
    /// </summary>
    public Vector3 GetValidSpawnPos()
    {
        // keep getting new position as long as asteroid is with 3 units of the player (0,0,0)
        Vector3 spawnPos;
        do
        {
            float randomXpos = Random.Range(spawnXMin, spawnXMax);
            float randomYpos = Random.Range(spawnYMin, spawnYMax);

            spawnPos = new Vector3(randomXpos, randomYpos, 0);
        } while (Vector3.Distance(spawnPos, Vector3.zero) < playerSafeDistance);

        return spawnPos;
    }

    /// <summary>
    /// Spawn an asteroid at the location specified by position parameter with the size specified by the size parameter.
    /// </summary>
    public void SpawnAsteroid(Vector3 position, Asteroid.AsteroidSize size)
    {
        Asteroid asteroidType;
        switch (size)
        {
            case Asteroid.AsteroidSize.Large:
                asteroidType = largeAsteroid;
                break;
            case Asteroid.AsteroidSize.Medium:
                asteroidType = mediumAsteroid;
                break;
            // for small and any other sizes, default to small
            case Asteroid.AsteroidSize.Small:
            default:
                asteroidType = smallAsteroid;
                break;

        }

        Instantiate(asteroidType, position, Quaternion.identity);
    }
}