using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 150f;
    public Rigidbody rb;

    [Header("Input Settings")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public bool useController = true; // Enable gamepad input

    private Vector3 moveDirection = Vector3.zero;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
        RotatePlayer();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;

        // KEYBOARD INPUT
        if (Input.GetKey(forwardKey)) verticalInput += 1f;
        if (Input.GetKey(backwardKey)) verticalInput -= 1f;
        if (Input.GetKey(rightKey)) horizontalInput += 1f;
        if (Input.GetKey(leftKey)) horizontalInput -= 1f;

        // GAMEPAD INPUT (if enabled)
        if (useController)
        {
            float gamepadHorizontal = Input.GetAxis("Horizontal");
            float gamepadVertical = Input.GetAxis("Vertical");

            horizontalInput += gamepadHorizontal;
            verticalInput += gamepadVertical;
        }

        // Clamp values
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);

        // If moving backward, flip the direction 180 degrees instead of reversing
        if (verticalInput < -0.1f)
        {
            verticalInput = 1f; // Always move forward
            horizontalInput = -horizontalInput; // Flip horizontal input for 180 turn
        }

        // Calculate movement direction
        moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
    }

    void RotatePlayer()
    {
        // Only rotate if moving
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void MovePlayer()
    {
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y; // Keep gravity
        rb.velocity = velocity;
    }
}