using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    [Header("Volgdoel (Player)")]
    public Transform player;   
    private CapsuleCollider playerCollider;

    [Header("Camera instellingen")]
    public Vector3 offset = new Vector3(0f, 2f, -7.75f);
    public float smoothSpeed = 5f;  

    private float initialZ;
    private static Camera instance;

    void Awake()
    {
        // ✅ Zorg dat er maar één camera blijft bestaan
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // camera blijft bestaan tussen scenes

        initialZ = transform.position.z;
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
        TryFindPlayer();
    }

    void TryFindPlayer()
    {
        // Zoek automatisch het object met tag "Player"
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

        // Houd de Z-positie constant (vaste diepte)
        targetPos.z = initialZ;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private Vector3 GetTargetPosition()
    {
        if (playerCollider != null)
        {
            // Gebruik midden van de collider voor vloeiende focus
            return player.position + player.up * (playerCollider.height * 0.5f);
        }

        return player.position;
    }
}
