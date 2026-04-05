using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetPlayer;
    public Vector3 offsetPosition = new Vector3(0, 1.5f, -2f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (targetPlayer == null) return;

        // Calculate desired position relative to player
        Vector3 desiredPosition = targetPlayer.position + targetPlayer.TransformDirection(offsetPosition);

        // Smoothly move camera to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // Camera looks at player (slightly above)
        Vector3 lookAtPoint = targetPlayer.position + Vector3.up * 0.5f;
        transform.LookAt(lookAtPoint);
    }
}