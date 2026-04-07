using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar Settings")]
    public Image healthBarImage; // The green bar fill
    public TMP_Text healthText; // Text showing "HP: X/Y"
    public float maxHealth = 100f;

    [Header("Appearance")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public float lowHealthThreshold = 0.3f; // Color change at 30% health

    private float currentHealth;
    private CanvasGroup canvasGroup; // For fade in/out

    void Start()
    {
        if (healthBarImage == null)
        {
            Debug.LogError("HealthBar: No health bar image assigned!");
            return;
        }

        currentHealth = maxHealth;
        canvasGroup = GetComponent<CanvasGroup>();

        UpdateHealthBar();
        Debug.Log("HealthBar initialized - Max Health: " + maxHealth);
    }

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth;

        // Update bar fill
        healthBarImage.fillAmount = healthPercent;

        // Change color based on health
        if (healthPercent <= lowHealthThreshold)
        {
            // Low health - red
            healthBarImage.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent / lowHealthThreshold);
        }
        else
        {
            // High health - green
            healthBarImage.color = fullHealthColor;
        }

        // Update text
        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(currentHealth) + " / " + Mathf.RoundToInt(maxHealth);
        }
    }
}