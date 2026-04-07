using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Movement")]
    public float bulletSpeed = 20f;
    public float lifetime = 5f;

    [Header("Steering")]
    public KeyCode steerLeftKey = KeyCode.A;
    public KeyCode steerRightKey = KeyCode.D;
    public float steerSpeed = 90f; // Degrees per second to rotate
    public bool useController = true;

    [Header("Collision")]
    public bool destroyOnPlayerHit = true;
    public bool destroyOnWallHit = true;

    private Vector3 moveDirection;
    private float spawnTime;
    private float currentYRotation = 0f; // Track current rotation
    private Rigidbody rb;

    void Start()
    {
        // Move in the direction the bullet is facing (forward)
        moveDirection = transform.forward;
        currentYRotation = transform.eulerAngles.y;
        spawnTime = Time.time;

        // Get rigidbody if it exists
        rb = GetComponent<Rigidbody>();

        // IMPORTANT: Make sure bullet is NOT a child of the player
        if (transform.parent != null)
        {
            transform.SetParent(null); // Detach from parent so it moves independently
            Debug.Log("Bullet detached from parent to move independently");
        }

        Debug.Log("Bullet spawned at " + transform.position + ", moving in direction: " + moveDirection);
    }

    void Update()
    {
        HandleSteering();
    }

    void FixedUpdate()
    {
        MoveBullet();
        CheckLifetime();
    }

    void HandleSteering()
    {
        // Check for continuous input (GetKey, not GetKeyDown)
        bool steerLeft = Input.GetKey(steerLeftKey);
        bool steerRight = Input.GetKey(steerRightKey);

        // Controller input - continuous
        if (useController)
        {
            float gamepadHorizontal = Input.GetAxis("Horizontal");
            if (gamepadHorizontal < -0.3f) steerLeft = true;
            if (gamepadHorizontal > 0.3f) steerRight = true;
        }

        // Steer continuously while holding key (like a car)
        if (steerLeft && !steerRight)
        {
            SteerBullet(-steerSpeed * Time.deltaTime);
        }
        else if (steerRight && !steerLeft)
        {
            SteerBullet(steerSpeed * Time.deltaTime);
        }
    }

    void SteerBullet(float angleOffset)
    {
        // Rotate around Y axis (horizontal plane)
        currentYRotation += angleOffset;

        // Update direction vector
        Quaternion rotation = Quaternion.Euler(0, currentYRotation, 0);
        moveDirection = rotation * Vector3.forward;

        // Rotate bullet visually
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }

    void MoveBullet()
    {
        // Move bullet using transform (independent of rigidbody)
        // This ensures bullet moves regardless of collisions with player
        transform.position += moveDirection * bulletSpeed * Time.fixedDeltaTime;
    }

    void CheckLifetime()
    {
        // Destroy bullet after X seconds if not destroyed by collision
        if (Time.time - spawnTime > lifetime)
        {
            Debug.Log("Bullet destroyed due to lifetime");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        HandleCollision(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject.GetComponent<Collider>());
    }

    void HandleCollision(Collider collision)
    {
        if (collision == null) return;

        string hitObjectName = collision.gameObject.name;
        string hitTag = collision.gameObject.tag;

        Debug.Log("Bullet hit: " + hitObjectName + " (Tag: " + hitTag + ")");

        // Check if hit a player (Player1 or Player2)
        if (hitObjectName.Contains("Player") || hitTag == "Player")
        {
            // Try to get PlayerHealth and damage it
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Damage amount
                Debug.Log("Bullet damaged " + hitObjectName);
            }

            Destroy(gameObject);
            return;
        }

        // Check if hit a wall or ground
        if (destroyOnWallHit && (hitObjectName.Contains("Ground") || hitObjectName.Contains("Wall") || hitTag == "Wall"))
        {
            Debug.Log("Bullet hit a wall! Player can shoot again.");
            Destroy(gameObject);
            return;
        }

        // Destroy on any collision by default
        Debug.Log("Bullet destroyed on collision");
        Destroy(gameObject);
    }
}





/*using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Movement")]
    public float bulletSpeed = 20f;
    public float lifetime = 5f;

    [Header("Steering")]
    public KeyCode steerLeftKey = KeyCode.A;
    public KeyCode steerRightKey = KeyCode.D;
    public float steerSpeed = 90f; // Degrees per second to rotate
    public bool useController = true;

    [Header("Collision")]
    public bool destroyOnPlayerHit = true;
    public bool destroyOnWallHit = true;

    private Vector3 moveDirection;
    private float spawnTime;
    private float currentYRotation = 0f; // Track current rotation

    void Start()
    {
        // Move in the direction the bullet is facing (forward)
        moveDirection = transform.forward;
        currentYRotation = transform.eulerAngles.y;
        spawnTime = Time.time;

        Debug.Log("Bullet spawned at " + transform.position + ", moving in direction: " + moveDirection);
    }

    void Update()
    {
        HandleSteering();
    }

    void FixedUpdate()
    {
        MoveBullet();
        CheckLifetime();
    }

    void HandleSteering()
    {
        // Check for continuous input (GetKey, not GetKeyDown)
        bool steerLeft = Input.GetKey(steerLeftKey);
        bool steerRight = Input.GetKey(steerRightKey);

        // Controller input - continuous
        if (useController)
        {
            float gamepadHorizontal = Input.GetAxis("Horizontal");
            if (gamepadHorizontal < -0.3f) steerLeft = true;
            if (gamepadHorizontal > 0.3f) steerRight = true;
        }

        // Steer continuously while holding key (like a car)
        if (steerLeft && !steerRight)
        {
            SteerBullet(-steerSpeed * Time.deltaTime);
        }
        else if (steerRight && !steerLeft)
        {
            SteerBullet(steerSpeed * Time.deltaTime);
        }
    }

    void SteerBullet(float angleOffset)
    {
        // Rotate around Y axis (horizontal plane)
        currentYRotation += angleOffset;

        // Update direction vector
        Quaternion rotation = Quaternion.Euler(0, currentYRotation, 0);
        moveDirection = rotation * Vector3.forward;

        // Rotate bullet visually
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }

    void MoveBullet()
    {
        // Move bullet forward constantly in current direction
        transform.position += moveDirection * bulletSpeed * Time.fixedDeltaTime;
    }

    void CheckLifetime()
    {
        // Destroy bullet after X seconds if not destroyed by collision
        if (Time.time - spawnTime > lifetime)
        {
            Debug.Log("Bullet destroyed due to lifetime");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        HandleCollision(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject.GetComponent<Collider>());
    }

    void HandleCollision(Collider collision)
    {
        if (collision == null) return;

        string hitObjectName = collision.gameObject.name;
        string hitTag = collision.gameObject.tag;

        Debug.Log("Bullet hit: " + hitObjectName + " (Tag: " + hitTag + ")");

        // Check if hit a player (Player1 or Player2)
        if (hitObjectName.Contains("Player") || hitTag == "Player")
        {
            // Try to get PlayerHealth and damage it
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Damage amount
            }

            Destroy(gameObject);
            return;
        }


        // Check if hit a wall or ground
        if (destroyOnWallHit && (hitObjectName.Contains("Ground") || hitObjectName.Contains("Wall") || hitTag == "Wall"))
        {
            Debug.Log("Bullet hit a wall! Player can shoot again.");
            Destroy(gameObject);
            return;
        }

        // Destroy on any collision by default
        Debug.Log("Bullet destroyed on collision");
        Destroy(gameObject);
    }
}






*//*using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Movement")]
    public float bulletSpeed = 20f;
    public float lifetime = 5f; // Destroy after 5 seconds if not hit

    [Header("Collision")]
    public bool destroyOnPlayerHit = true; // Destroy when hitting player
    public bool destroyOnWallHit = true;   // Destroy when hitting wall/obstacle

    private Vector3 moveDirection;
    private float spawnTime;

    void Start()
    {
        // Move in the direction the bullet is facing (forward)
        moveDirection = transform.forward;
        spawnTime = Time.time;

        Debug.Log("Bullet spawned, moving in direction: " + moveDirection);
    }

    void FixedUpdate()
    {
        MoveBullet();
        CheckLifetime();
    }

    void MoveBullet()
    {
        // Move bullet forward constantly
        transform.position += moveDirection * bulletSpeed * Time.fixedDeltaTime;
    }

    void CheckLifetime()
    {
        // Destroy bullet after X seconds if not destroyed by collision
        if (Time.time - spawnTime > lifetime)
        {
            Debug.Log("Bullet destroyed due to lifetime");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        HandleCollision(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject.GetComponent<Collider>());
    }

    void HandleCollision(Collider collision)
    {
        if (collision == null) return;

        string hitObjectName = collision.gameObject.name;
        string hitTag = collision.gameObject.tag;

        Debug.Log("Bullet hit: " + hitObjectName + " (Tag: " + hitTag + ")");

        // Check if hit a player (Player1 or Player2)
        if (destroyOnPlayerHit && (hitObjectName.Contains("Player") || hitTag == "Player"))
        {
            Debug.Log("Bullet hit a player!");
            Destroy(gameObject);
            return;
        }

        // Check if hit a wall or ground
        if (destroyOnWallHit && (hitObjectName.Contains("Ground") || hitObjectName.Contains("Wall") || hitTag == "Wall"))
        {
            Debug.Log("Bullet hit a wall!");
            Destroy(gameObject);
            return;
        }

        // Destroy on any collision by default
        Debug.Log("Bullet destroyed on collision");
        Destroy(gameObject);
    }
}*/