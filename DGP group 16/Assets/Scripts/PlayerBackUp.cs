using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerBackUp : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float jumpForce = 5f;
    public CameraPresetManager presetManager;
    [SerializeField] private Transform spookje;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool isGrounded;

    private float growthDuration = 120f;   // 2 minutes
    private float targetScaleMultiplier = 1.5f; // grow 1.5× original size
    private static float growthTimer = 0f;
    private Vector3 initialScale;
    private float initialColliderHeight;
    private float rotationSpeed = 500; // How fast the character turns to the side their moving towards.
    private Vector3 initialScaleSpookje;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        capsule = GetComponent<CapsuleCollider>();
        initialScale = transform.localScale;
        initialColliderHeight = capsule.height;

        initialScaleSpookje = spookje.localScale;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleGrowth();
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

    private void HandleGrowth()
    {
        if (growthTimer < growthDuration)
        {
            growthTimer += Time.deltaTime;
            float t = Mathf.Clamp01(growthTimer / growthDuration);

            // Gradually scale up to 10× original
            float scaleMultiplier = Mathf.Lerp(1f, targetScaleMultiplier, t);
            transform.localScale = initialScale * scaleMultiplier;

            // Adjust capsule collider height accordingly
            float newHeight = Mathf.Lerp(initialColliderHeight, initialColliderHeight * targetScaleMultiplier, t);
            capsule.height = newHeight;
            capsule.center = new Vector3(0, newHeight / 2f, 0);

            // Adjust visual size
            spookje.localScale = initialScaleSpookje * scaleMultiplier;
        }
        else
        {
            SceneManager.LoadScene("Startscherm");
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
