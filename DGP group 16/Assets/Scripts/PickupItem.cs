using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PickupItem : MonoBehaviour
{
    [Header("Hold visual")]
    public Vector3 holdOffset   = new Vector3(0f, 0.2f, 0.5f);
    public Vector3 holdRotation = Vector3.zero;

    [Header("Behaviour")]
    public bool carryBetweenScenes = true;      // travels with persistent Player
    [Tooltip("Enable for the RED gear so the system can stop/start.")]
    public bool notifyGearSystem = false;

    [Header("Start State")]
    public bool startInSocket = false;          // gear starts already mounted
    public GearSocket startSocket = null;       // optional: snap here on Start

    [Header("Optional (auto-filled if null)")]
    public GearSystemController systemController;

    // runtime state
    [HideInInspector] public bool isMountedInSocket = false;

    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb  = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (notifyGearSystem && systemController == null)
            systemController = FindAnyObjectByType<GearSystemController>();
    }

    void Start()
    {
        if (startInSocket && startSocket != null)
        {
            // snap immediately so it doesn't fall
            SnapToSocket(startSocket);
        }
        else if (startInSocket && startSocket == null)
        {
            // mounted but no socket reference: freeze where it is
            rb.isKinematic = true;
            rb.useGravity  = false;
            col.enabled    = true;      // keep collider ON so we can pick it later
            isMountedInSocket = true;
        }
    }

    // called by PlayerPickUp on P
    public void PickUp(Transform holdPoint)
    {
        rb.linearVelocity        = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity  = false;
        col.enabled    = false;       // collider OFF while carrying

        transform.SetParent(holdPoint);
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.Euler(holdRotation);

        isMountedInSocket = false;

        if (notifyGearSystem && systemController != null)
            systemController.OnRedGearPickedUp();
    }

    // free drop (no socket nearby)
    public void DropFree(Vector3 worldPos, Vector3 impulse)
    {
        transform.SetParent(null);
        transform.position = worldPos;

        rb.isKinematic = false;
        rb.useGravity  = true;
        col.enabled    = true;

        if (impulse.sqrMagnitude > 0f)
            rb.AddForce(impulse, ForceMode.Impulse);

        if (carryBetweenScenes)
        {
            Scene active = SceneManager.GetActiveScene();
            SceneManager.MoveGameObjectToScene(gameObject, active);
        }

        isMountedInSocket = false;
    }

    // snap into a wall socket
    public void SnapToSocket(GearSocket socket)
    {
        rb.isKinematic = true;
        rb.useGravity  = false;
        col.enabled    = true;  // collider ON while mounted so we can pick up again

        Transform parent = socket.mountPoint != null ? socket.mountPoint : socket.transform;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isMountedInSocket = true;

        if (notifyGearSystem && systemController != null)
            systemController.OnRedGearReturned();

        socket.OnItemSnapped(this);
    }
}
