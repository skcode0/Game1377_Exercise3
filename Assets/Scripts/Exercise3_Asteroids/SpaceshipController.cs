/*
 * Assignment: AsteroidsGame - SpaceshipController Script - PART 1 & 2
 * 
 * Objective:
 * Implement a player controller for a spaceship in an Asteroids prototype. The player should be able to rotate the ship,
 * move forward, wrap around the screen, and shoot bullets. 
 * 
 * Requirements:
 * PART 1: Player Movement
 * 1. The player should be able to rotate the ship left and right using A/D keys from an input axis.
 *      This movement should be done with Transform based movement. 
 * 2. The player should be able to thrust forward using only the W key from an input axis
 *      This movement should be done with physics applied to a RigidBody2D. 
 * 3. The player should be able to wrap around the screen when they go off one edge and come back on the other side.
 * 4. The player should be able to teleport to a random location on the screen using left shift in an input button. You 
 *      do not need to check if there is an asteroid there. 
 *      Hint: For determining the random location, you can use the ScreenBounds class (see ScreenWrap.cs for how to use)
 *      
 * PART 2: Shooting
 * 1. The player should be able to shoot bullets using the space key in an input button
 *      Bullets should only go in the direction the ship is facing and bullet speed should be controlled by the Bullet.cs
 
 */

using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 500f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private float rotationInput;
    private float thrustInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        HandleRotation();
        HandleFire();
        HandleHyperspace();
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    /// <summary>
    /// Rotates the ship by pressing horizontal controls
    /// </summary>
    private void HandleRotation()
    {
        transform.Rotate(0, 0, -rotationInput * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Trust forward using W key. Movement is based on physics.
    /// </summary>

    private void HandleThrust()
    {
        if (thrustInput > 0)
        {
            rb.AddForce(transform.up * thrustInput * thrustForce, ForceMode2D.Force);
        }
    }

    /// <summary>
    /// When pressing Space, fire bullet
    /// </summary>
    private void HandleFire()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FireBullet();
        }
    }

    /// <summary>
    /// Spawn bullet
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    /// <summary>
    /// Handle random teleportation from pressing left shift
    /// </summary>
    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            TeleportToRandomLocation();
        }
    }

    /// <summary>
    /// Teleport to random location
    /// </summary>
    private void TeleportToRandomLocation()
    {
        float randomX = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomY = Random.Range(ScreenBounds.ScreenTop, ScreenBounds.ScreenBottom);

        transform.position = new Vector3(randomX, randomY, 0);
    }
}
