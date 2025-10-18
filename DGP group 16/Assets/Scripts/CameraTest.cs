using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTest : MonoBehaviour
{
    [Header("Volgdoel (Player)")]
    public Transform player;
    private CapsuleCollider playerCollider;

    [Header("Camera instellingen")]
    public Vector3 offset = new Vector3(0f, 2f, -7.75f);
    public float smoothSpeed = 5f;

    private Vector3 startPosition; // startpositie van de camera per scene
    private static CameraTest instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        startPosition = transform.position;
        TryFindPlayer();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startPosition = transform.position;
        TryFindPlayer();
    }

    void TryFindPlayer()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
                playerCollider = player.GetComponent<CapsuleCollider>();
            }
        }
    }

    void LateUpdate()
    {
        if (player == null)
        {
            TryFindPlayer();
            return;
        }

        Vector3 targetPos = GetTargetPosition() + offset;

        // Z-logica: camera volgt alleen als speler naar camera toe beweegt
        // (speler Z kleiner dan camera Z + offset.z)
        float cameraZ = transform.position.z;
        float playerZ = player.position.z + offset.z;

        if (playerZ < cameraZ) // speler komt dichterbij camera
        {
            targetPos.z = playerZ;
        }
        else
        {
            targetPos.z = startPosition.z; // speler loopt weg, camera blijft op start
        }

        // Smooth Lerp
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private Vector3 GetTargetPosition()
    {
        if (playerCollider != null)
        {
            return player.position + player.up * (playerCollider.height * 0.5f);
        }
        return player.position;
    }
}
