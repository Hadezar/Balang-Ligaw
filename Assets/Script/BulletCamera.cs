using UnityEngine;

public class BulletCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera bulletCamera;
    public float cameraDistance = 0.5f; // How far behind bullet
    public Vector2 viewportPosition = new Vector2(0.5f, 0.1f); // Center bottom
    public Vector2 viewportSize = new Vector2(0.25f, 0.25f); // 25% of screen

    private Transform bulletTransform;

    void Start()
    {
        bulletTransform = GetComponent<Transform>();

        if (bulletCamera == null)
        {
            Debug.LogError("BulletCamera: No camera assigned!");
            return;
        }

        // Set camera as child of bullet (already done in editor ideally)
        if (bulletCamera.transform.parent != transform)
        {
            bulletCamera.transform.SetParent(transform);
        }

        // Position camera behind bullet
        bulletCamera.transform.localPosition = new Vector3(0, 0, -cameraDistance);
        bulletCamera.transform.localRotation = Quaternion.identity;

        Debug.Log("BulletCamera initialized");
    }

    void LateUpdate()
    {
        UpdateCameraViewport();
    }

    void UpdateCameraViewport()
    {
        if (bulletCamera == null) return;

        Rect viewportRect = new Rect(
            viewportPosition.x - (viewportSize.x / 2f), // Center horizontally
            viewportPosition.y,                          // Bottom position
            viewportSize.x,
            viewportSize.y
        );

        bulletCamera.rect = viewportRect;
    }
}