using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Hit Settings")]
    public float damagePerHit = 10f;
    public float invulnerabilityDuration = 0.5f; // Seconds of invulnerability after hit

    [Header("Visual Feedback")]
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.2f;

    private float lastHitTime = -999f;
    private bool isInvulnerable = false;
    private Renderer playerRenderer;
    private Color originalColor;

    // Reference for HealthBar
    [HideInInspector] public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // Get the capsule renderer for hit flash
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        // Find HealthBar as child
        HealthBar hb = GetComponentInChildren<HealthBar>();
        if (hb != null)
        {
            healthBar = hb;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth: No HealthBar found as child!");
        }

        Debug.Log(gameObject.name + " Health: " + currentHealth + "/" + maxHealth);
    }

    void Update()
    {
        // Check if invulnerability period is over
        if (isInvulnerable && Time.time - lastHitTime > invulnerabilityDuration)
        {
            isInvulnerable = false;
            ResetPlayerColor();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        HandleBulletHit(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider col = collision.gameObject.GetComponent<Collider>();
        if (col != null)
        {
            HandleBulletHit(col);
        }
    }

    void HandleBulletHit(Collider collision)
    {
        // Ignore if invulnerable
        if (isInvulnerable)
        {
            Debug.Log("Hit but invulnerable!");
            return;
        }

        string hitObjectName = collision.gameObject.name;

        // Check if hit by a bullet
        if (hitObjectName.Contains("Bullet"))
        {
            TakeDamage(damagePerHit);
            Debug.Log(gameObject.name + " hit by bullet! Health: " + currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Set invulnerability
        isInvulnerable = true;
        lastHitTime = Time.time;

        // Update healthbar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        // Visual feedback - flash red
        FlashRed();

        Debug.Log(gameObject.name + " took " + damage + " damage! Health: " + currentHealth);

        // Check if dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        Debug.Log(gameObject.name + " healed " + amount + "! Health: " + currentHealth);
    }

    void FlashRed()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = hitColor;
            // Will be reset in Update when invulnerability expires
        }
    }

    void ResetPlayerColor()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " is DEAD! Destroying player...");
        // Destroy the entire player gameobject
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}



/*using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Hit Settings")]
    public float damagePerHit = 10f;
    public float invulnerabilityDuration = 0.5f; // Seconds of invulnerability after hit

    [Header("Visual Feedback")]
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.2f;

    private float lastHitTime = -999f;
    private bool isInvulnerable = false;
    private Renderer playerRenderer;
    private Color originalColor;

    // Reference for HealthBar
    [HideInInspector] public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // Get the capsule renderer for hit flash
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        // Find HealthBar as child
        HealthBar hb = GetComponentInChildren<HealthBar>();
        if (hb != null)
        {
            healthBar = hb;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth: No HealthBar found as child!");
        }

        Debug.Log(gameObject.name + " Health: " + currentHealth + "/" + maxHealth);
    }

    void Update()
    {
        // Check if invulnerability period is over
        if (isInvulnerable && Time.time - lastHitTime > invulnerabilityDuration)
        {
            isInvulnerable = false;
            ResetPlayerColor();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        HandleBulletHit(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider col = collision.gameObject.GetComponent<Collider>();
        if (col != null)
        {
            HandleBulletHit(col);
        }
    }

    void HandleBulletHit(Collider collision)
    {
        // Ignore if invulnerable
        if (isInvulnerable)
        {
            Debug.Log("Hit but invulnerable!");
            return;
        }

        string hitObjectName = collision.gameObject.name;

        // Check if hit by a bullet
        if (hitObjectName.Contains("Bullet"))
        {
            TakeDamage(damagePerHit);
            Debug.Log(gameObject.name + " hit by bullet! Health: " + currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Set invulnerability
        isInvulnerable = true;
        lastHitTime = Time.time;

        // Update healthbar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        // Visual feedback - flash red
        FlashRed();

        Debug.Log(gameObject.name + " took " + damage + " damage! Health: " + currentHealth);

        // Check if dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        Debug.Log(gameObject.name + " healed " + amount + "! Health: " + currentHealth);
    }

    void FlashRed()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = hitColor;
            // Will be reset in Update when invulnerability expires
        }
    }

    void ResetPlayerColor()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " is DEAD!");
        // TODO: Add death animation, respawn, or end game logic
        // For now, just disable movement
        // Disable this player's controller scripts
        GetComponent<PlayerWalk>().enabled = false;
        GetComponent<PlayerShooting>().enabled = false;
        GetComponent<PlayerJump>().enabled = false;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}*/