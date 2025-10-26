using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    private float jumpForce = 6f;
    public CameraPresetManager presetManager;
    [SerializeField] private Transform spookje;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool isGrounded;

    private float rotationSpeed = 500; // How fast the character turns to the side their moving towards.

    private float fadeDuration = 2700;
    private static float fadeTimer = 0f;
    private Material spookjeMaterial;
    private Color initialColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        capsule = GetComponent<CapsuleCollider>();

        // Get materials to fade out player
        Renderer rend = spookje.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            spookjeMaterial = rend.material;
            initialColor = spookjeMaterial.color;
        } else
        {
            Debug.LogWarning("Player does not have a renderer");
        }
        
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFade();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveDirection.z += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveDirection.z -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveDirection.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveDirection.x += 1;

        moveDirection = moveDirection.normalized;
        Vector3 desiredVelocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(-moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        // keep gravity/jump velocity
        rb.linearVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void HandleFade()
    {
        if (spookjeMaterial == null) return;

        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);

        // Fade from 1 to 0
        Color newColor = initialColor;
        newColor.a = Mathf.Lerp(1f, 0f, t);
        spookjeMaterial.color = newColor;

        if (fadeTimer >= fadeDuration)
        {
            // this probably has to change to a end-scene thing.
            SceneManager.LoadScene("Startscherm");
            spookjeMaterial.color = initialColor;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
