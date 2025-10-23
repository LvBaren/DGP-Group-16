using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SpringMovement : MonoBehaviour
{
    [Header("Settings")]
    private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private string wallTag = "Wall";

    private Rigidbody rb;
    private Collider myCollider;
    private Transform visualSpring;
    private float rotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        visualSpring = transform.GetChild(0);

        // Make it immune to physics pushes
        rb.isKinematic = true;

        // Compute visual rotation speed (rolling look)
        rotationSpeed = (moveSpeed / radius) * Mathf.Rad2Deg;
    }

    void Update()
    {
        Vector3 move = new Vector3(-moveSpeed, 0f, 0f);
        transform.Translate(move * Time.deltaTime, Space.World);

        visualSpring.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);

        if (transform.position.x < -23)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore walls (so it passes through)
        if (collision.collider.CompareTag(wallTag))
        {
            Physics.IgnoreCollision(myCollider, collision.collider);
        }
    }
}
