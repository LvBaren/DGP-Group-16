using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SpringMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private string wallTag = "Wall";

    private Rigidbody rb;
    private Collider myCollider;
    private Transform visualSpring;
    private float rotationSpeed;

    private Transform spawnPoint;
    private Transform exitPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        visualSpring = transform.GetChild(0);

        rb.isKinematic = true; // Beweging zelf regelen
        rotationSpeed = (moveSpeed / radius) * Mathf.Rad2Deg;

        // Zoek automatisch naar de spawn en exit planes in de scene
        GameObject spawnObj = GameObject.FindGameObjectWithTag("SpringSpawn");
        GameObject exitObj = GameObject.FindGameObjectWithTag("SpringExit");

        if (spawnObj != null) spawnPoint = spawnObj.transform;
        if (exitObj != null) exitPoint = exitObj.transform;
    }

    void Update()
    {
        // Laat de veer bewegen
        Vector3 move = new Vector3(-moveSpeed, 0f, 0f);
        transform.Translate(move * Time.deltaTime, Space.World);

        // Laat hem draaien
        visualSpring.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);

        // Controleer of we voorbij het Exit-punt zijn
        if (exitPoint != null && transform.position.x < exitPoint.position.x)
        {
            TeleportToSpawn();
        }
    }

    private void TeleportToSpawn()
    {
        if (spawnPoint == null) return;

        // Behoud Y en Z van huidige positie
        Vector3 newPosition = new Vector3(
            spawnPoint.position.x,
            transform.position.y,
            transform.position.z
        );

        transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(wallTag))
        {
            Physics.IgnoreCollision(myCollider, collision.collider);
        }
    }
}