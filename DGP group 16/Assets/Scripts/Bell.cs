using UnityEngine;

public class Bell : MonoBehaviour
{
    private float swingAngle = 20f;   // Maximum rotation angle (degrees)
    private float swingSpeed = 2f;    // Speed of swinging

    private float initialZ;          // Store initial rotation

    void Start()
    {
        // Store initial rotation around Z-axis
        initialZ = transform.eulerAngles.z;
    }

    void Update()
    {
        // Calculate new rotation using sine wave
        float angle = swingAngle * Mathf.Sin(Time.time * swingSpeed);

        // Apply rotation (assuming bell swings around Z-axis)
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, initialZ + angle);
    }
}
