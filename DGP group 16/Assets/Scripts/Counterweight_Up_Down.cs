using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Counterweight : MonoBehaviour
{
    [Header("Beweging instellingen")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float height = 0.5f;

    private Rigidbody rb;
    private Vector3 startPos;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;              // we besturen hem zelf
        rb.interpolation = RigidbodyInterpolation.Interpolate; // vloeiender visueel
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        // Gebruik fixedTime voor vloeiende beweging in de physics-loop
        float newY = Mathf.Sin(Time.fixedTime * speed) * height + startPos.y;
        Vector3 newPos = new Vector3(startPos.x, newY, startPos.z);

        rb.MovePosition(newPos);
    }
}

