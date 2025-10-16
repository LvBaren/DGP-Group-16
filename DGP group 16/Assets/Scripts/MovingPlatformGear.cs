using UnityEngine;

public class MovingPlatformGear : MonoBehaviour
{
    [Header("Beweging")]
    public Vector3 direction = Vector3.up; // Richting (Up = verticaal)
    public float distance = 1.5f;          // Hoe ver op/neer
    public float speed = 1.2f;             // Snelheid
    public bool centered = true;           // Beweeg rond startpositie

    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
        direction = direction.normalized;
    }

    void Update()
    {
        if (centered)
        {
            // Beweegt rond de startpositie met sinus
            float offset = Mathf.Sin(Time.time * speed) * (distance * 0.5f);
            transform.position = startPos + direction * offset;
        }
        else
        {
            // Beweegt tussen startPos en startPos + distance
            float t = Mathf.PingPong(Time.time * speed, 1f);
            transform.position = startPos + direction * (t * distance);
        }
    }
}
