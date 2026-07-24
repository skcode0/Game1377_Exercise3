using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner Instance { get; private set; }
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private int spawnAmount = 3;
    [SerializeField] private float respawnDelay = 5f;
    private int currentPowerUpAmount;

    // These variables determine the spawn area for the asteroids.
    // They are calculated at Start based off of the camera size. 
    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;
    private float playerDistance = 1f;

    void Start()
    {
        float randomX = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomY = Random.Range(ScreenBounds.ScreenTop, ScreenBounds.ScreenBottom);
        spawnXMax = randomX + playerDistance;
        spawnXMin = -randomX - playerDistance;
        spawnYMax = randomY + playerDistance;
        spawnYMin = -randomY - playerDistance;

        SpawnPowerUp(spawnAmount);
        currentPowerUpAmount = spawnAmount;

        StartCoroutine(CheckPowerUps());
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Get valid spawn position
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
        } while (Vector3.Distance(spawnPos, Vector3.zero) < playerDistance);

        return spawnPos;
    }

    /// <summary>
    /// Spawn power up
    /// </summary>
    /// <param name="amount">define how many should be spawned</param>
    public void SpawnPowerUp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = GetValidSpawnPos();
            int index = Random.Range(0, powerUps.Length);

            Instantiate(powerUps[index], spawnPos, Quaternion.identity);
        }
    }

    private IEnumerator CheckPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnDelay);

            if (currentPowerUpAmount < spawnAmount)
            {
                int amountToSpawn = spawnAmount - currentPowerUpAmount;

                SpawnPowerUp(amountToSpawn);
                currentPowerUpAmount += amountToSpawn;
            }
        }
    }

    /// <summary>
    /// Update power up amount
    /// </summary>
    public void PowerUpCollected()
    {
        currentPowerUpAmount--;
    }
}