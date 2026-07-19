using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Rigidbody spaceShip;
    public static bool isPlayerDead = false;
    private float invincibilityTime = 3.0f;

    void Update()
    {
        if (isPlayerDead)
        {
            Instantiate(spaceShip, Vector3.zero, Quaternion.identity);
            isPlayerDead = false;
        }
    }

// IEnumerator InvicibilityRoutine(float invincibilityTime)
//     {
        
//     }

}