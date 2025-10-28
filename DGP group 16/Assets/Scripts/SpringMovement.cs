using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SpringMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private string wallTag = "Wall";

    [Header("Audio")]
    [SerializeField] private AudioClip rollSound; // AudioClip voor rollen
    [SerializeField] private float maxVolume = 1f; // Volume dicht bij speler
    [SerializeField] private float minVolumeDistance = 10f; // Afstand waar volume minimaal is
    [SerializeField] private float maxVolumeDistance = 2f;  // Afstand waar volume maximaal is

    private Rigidbody rb;
    private Collider myCollider;
    private Transform visualSpring;
    private float rotationSpeed;
    private Transform spawnPoint;
    private Transform exitPoint;

    private AudioSource audioSource;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        visualSpring = transform.GetChild(0);

        rb.isKinematic = true;
        rotationSpeed = (moveSpeed / radius) * Mathf.Rad2Deg;

        // Zoek automatisch naar de spawn en exit planes
        GameObject spawnObj = GameObject.FindGameObjectWithTag("SpringSpawn");
        GameObject exitObj = GameObject.FindGameObjectWithTag("SpringExit");
        if (spawnObj != null) spawnPoint = spawnObj.transform;
        if (exitObj != null) exitPoint = exitObj.transform;

        // Audio setup
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rollSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D geluid
        audioSource.minDistance = maxVolumeDistance;
        audioSource.maxDistance = minVolumeDistance;
        audioSource.volume = 0f;

        // Vind speler voor afstandsberekening
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (rollSound != null)
            audioSource.Play(); // Start looping
    }

    void Update()
    {
        // Beweging
        Vector3 move = new Vector3(-moveSpeed, 0f, 0f);
        transform.Translate(move * Time.deltaTime, Space.World);

        // Rotatie
        visualSpring.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);

        // Past volume aan op afstand tot speler
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            float vol = Mathf.Clamp01(1 - (distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
            audioSource.volume = vol * maxVolume;
        }

        // Teleport als voorbij exit
        if (exitPoint != null && transform.position.x < exitPoint.position.x)
        {
            TeleportToSpawn();
        }
    }

    private void TeleportToSpawn()
    {
        if (spawnPoint == null) return;

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