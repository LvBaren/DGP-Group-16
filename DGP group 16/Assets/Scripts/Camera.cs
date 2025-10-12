using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Assign your player object here
    public Vector3 offset;     // Adjust to position camera behind/above the player
    public float smoothSpeed = 5f;  // Controls how smoothly the camera follows

    private CapsuleCollider playerCollider;

    void Start()
    {
        if (player != null)
        {
            playerCollider = player.GetComponent<CapsuleCollider>();
            transform.position = GetTargetPosition() + offset;
        }
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Use midpoint of player collider instead of player transform origin
        Vector3 targetPos = GetTargetPosition() + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Optional: camera faces the player
        // transform.LookAt(GetTargetPosition());
    }

    private Vector3 GetTargetPosition()
    {
        if (playerCollider != null)
        {
            // Middle point = player position + (collider height * 0.5 * player's up direction)
            return player.position + player.up * (playerCollider.height * 0.5f);
        }

        return player.position;
    }
}