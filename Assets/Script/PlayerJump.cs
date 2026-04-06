using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public KeyCode jumpKey = KeyCode.Space;
    public bool useController = true;

    [Header("Ground Detection")]
    public float groundDrag = 5f;
    public float airDrag = 2f;
    public float raycastDistance = 0.6f; // Make this adjustable
    private bool isGrounded = false;

    [Header("Landing Shake")]
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 15f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private Vector3 cameraOriginalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerJump: No Rigidbody found on " + gameObject.name);
            return;
        }

        // Find camera as child of this player
        cameraTransform = transform.Find("EyeCamera");
        if (cameraTransform == null)
        {
            Debug.LogWarning("PlayerJump: No EyeCamera found as child of player!");
        }
        else
        {
            cameraOriginalPos = cameraTransform.localPosition;
            Debug.Log("PlayerJump: Camera found at " + cameraOriginalPos);
        }

        Debug.Log("PlayerJump initialized on " + gameObject.name);
    }

    void Update()
    {
        HandleJumpInput();
        CheckGroundStatus();
    }

    void FixedUpdate()
    {
        ApplyDrag();
    }

    void HandleJumpInput()
    {
        bool jumpPressed = Input.GetKeyDown(jumpKey);

        // Controller input (A button on most gamepads)
        if (useController)
        {
            jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        }

        if (jumpPressed)
        {
            Debug.Log("Jump pressed! isGrounded: " + isGrounded);
        }

        if (jumpPressed && isGrounded)
        {
            Debug.Log("JUMPING!");
            Jump();
        }
        else if (jumpPressed && !isGrounded)
        {
            Debug.Log("Jump attempted but NOT grounded");
        }
    }

    void CheckGroundStatus()
    {
        // Cast a ray downward to check if player is on ground
        bool wasGrounded = isGrounded;

        // Raycast from center of player downward
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);

        if (isGrounded)
        {
            Debug.Log("Grounded: Hit " + hit.collider.gameObject.name);
        }

        // If just landed (was in air, now grounded)
        if (!wasGrounded && isGrounded)
        {
            Debug.Log("LANDED! Starting shake...");
            StartCoroutine(LandingShake());
        }
    }

    void Jump()
    {
        // Add upward force
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset Y velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Jump applied with force: " + jumpForce);
    }

    void ApplyDrag()
    {
        // Increase drag when grounded (for snappy feel)
        // Decrease drag in air (for float feel)
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    IEnumerator LandingShake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Create up/down shake motion
            float shakeOffset = Mathf.Sin(elapsedTime * shakeSpeed * Mathf.PI) * shakeAmount;

            // Apply shake to camera local position
            if (cameraTransform != null)
            {
                Vector3 shakePos = cameraOriginalPos;
                shakePos.y += shakeOffset;
                cameraTransform.localPosition = shakePos;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset camera to original position
        if (cameraTransform != null)
        {
            cameraTransform.localPosition = cameraOriginalPos;
        }
    }
}