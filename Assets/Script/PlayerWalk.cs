using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float turnSpeed = 100f;
    public Rigidbody rb;

    [Header("Input Settings")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public bool useController = true;

    private Vector3 moveDirection = Vector3.zero;
    private float horizontalInput = 0f;

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

        // KEYBOARD INPUT
        if (Input.GetKey(leftKey)) horizontalInput = -1f;    // A = turn left
        if (Input.GetKey(rightKey)) horizontalInput = 1f;    // D = turn right

        // GAMEPAD INPUT (if enabled)
        if (useController)
        {
            horizontalInput += Input.GetAxis("Horizontal");   // Left stick horizontal
        }

        // Clamp to -1 to 1
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

        // W = always move forward in the direction we're facing
        if (Input.GetKey(forwardKey))
        {
            moveDirection = transform.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void RotatePlayer()
    {
        // Rotate left or right based on horizontal input
        if (horizontalInput != 0)
        {
            transform.Rotate(0, horizontalInput * turnSpeed * Time.deltaTime, 0);
        }
    }

    void MovePlayer()
    {
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y; // Keep gravity
        rb.velocity = velocity;
    }
}