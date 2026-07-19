using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spaceShip;
    public static bool isPlayerDead = false;
    private float invincibilityTime = 3.0f;

    void Update()
    {
        if (isPlayerDead && PlayerStats.playerLives > 0)
        {
            Instantiate(spaceShip, Vector3.zero, Quaternion.identity);
            isPlayerDead = false;
            StartCoroutine(RespawnInvicibility());
        }
    }

    IEnumerator RespawnInvicibility()
    {
        Asteroid.canKillPlayer = false;

        yield return new WaitForSeconds(invincibilityTime);

        Asteroid.canKillPlayer = true;
    }
}