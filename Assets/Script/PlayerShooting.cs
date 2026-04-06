using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float shootCooldown = 0.2f; // Delay between shots (just for spam prevention)
    public KeyCode shootKey = KeyCode.Mouse0;
    public bool useController = true;

    [Header("Spawn Point")]
    public Transform shootSpawnPoint;
    public float spawnDistance = 2f;

    [Header("Shooting State")]
    public bool canShoot = true; // Can shoot immediately
    private float lastShootTime = 0f;
    private Transform cameraTransform;
    private GameObject currentBullet = null; // Track current bullet

    void Start()
    {
        cameraTransform = transform.Find("EyeCamera");
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerShooting: No EyeCamera found!");
            return;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("PlayerShooting: No bullet prefab assigned!");
        }

        Debug.Log("PlayerShooting initialized on " + gameObject.name);
    }

    void Update()
    {
        HandleShootInput();
        CheckBulletStatus(); // Check if current bullet is destroyed
    }

    void HandleShootInput()
    {
        bool shootPressed = Input.GetKeyDown(shootKey);

        // Controller input
        if (useController)
        {
            shootPressed = shootPressed || Input.GetButtonDown("Fire1");
        }

        if (shootPressed)
        {
            TryShoot();
        }
    }

    void CheckBulletStatus()
    {
        // If current bullet was destroyed, player can shoot again
        if (currentBullet == null && !canShoot)
        {
            Debug.Log("Bullet destroyed! Player can shoot again.");
            canShoot = true;
        }
    }

    void TryShoot()
    {
        // Can only shoot if no bullet in flight
        if (!canShoot)
        {
            Debug.Log("Already have a bullet in flight! Wait for it to hit something.");
            return;
        }

        // Check cooldown for spam prevention (very short delay)
        if (Time.time - lastShootTime < shootCooldown)
        {
            return;
        }

        lastShootTime = Time.time;

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform not found!");
            return;
        }

        // Determine spawn position
        Vector3 spawnPos;

        if (shootSpawnPoint != null)
        {
            spawnPos = shootSpawnPoint.position;
        }
        else
        {
            spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;
        }

        Quaternion spawnRotation = cameraTransform.rotation;

        currentBullet = Instantiate(bulletPrefab, spawnPos, spawnRotation);
        canShoot = false; // Can't shoot until this bullet is destroyed

        Debug.Log("Bullet spawned at " + spawnPos + ". Player cannot shoot again until bullet is destroyed.");
    }
}






/*using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float shootCooldown = 0.2f; // Delay between shots
    public KeyCode shootKey = KeyCode.Mouse0; // Left mouse button
    public bool useController = true;

    [Header("Spawn Point")]
    public Transform shootSpawnPoint; // Assign manually or create one
    public float spawnDistance = 2f; // How far in front of camera to spawn

    private float lastShootTime = 0f;
    private Transform cameraTransform;

    void Start()
    {
        // Find camera as child of this player
        cameraTransform = transform.Find("EyeCamera");
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerShooting: No EyeCamera found!");
            return;
        }

        // If no spawn point assigned, create one at spawn distance
        if (shootSpawnPoint == null)
        {
            Debug.LogWarning("PlayerShooting: No spawn point assigned. Creating default spawn point.");
            // Spawn point is spawnDistance in front of camera
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("PlayerShooting: No bullet prefab assigned!");
        }

        Debug.Log("PlayerShooting initialized on " + gameObject.name);
    }

    void Update()
    {
        HandleShootInput();
    }

    void HandleShootInput()
    {
        bool shootPressed = Input.GetKeyDown(shootKey);

        // Controller input (Right Trigger on most gamepads - Fire1)
        if (useController)
        {
            shootPressed = shootPressed || Input.GetButtonDown("Fire1");
        }

        if (shootPressed)
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        // Check if enough time has passed since last shot
        if (Time.time - lastShootTime < shootCooldown)
        {
            return; // Too soon, can't shoot yet
        }

        lastShootTime = Time.time;

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform not found!");
            return;
        }

        // Determine spawn position
        Vector3 spawnPos;

        if (shootSpawnPoint != null)
        {
            // Use assigned spawn point
            spawnPos = shootSpawnPoint.position;
        }
        else
        {
            // Use default: spawnDistance in front of camera
            spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;
        }

        // Spawn rotation faces forward (camera direction)
        Quaternion spawnRotation = cameraTransform.rotation;

        GameObject newBullet = Instantiate(bulletPrefab, spawnPos, spawnRotation);

        Debug.Log("Bullet spawned at " + spawnPos + " facing " + cameraTransform.forward);
    }
}*/