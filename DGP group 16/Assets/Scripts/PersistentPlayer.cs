using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer _instance;
    private Rigidbody rb;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (_instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start coroutine om spawn te verwerken
        StartCoroutine(SpawnAtPoint());
    }

    private IEnumerator SpawnAtPoint()
    {
        // Wacht tot einde van frame, zodat colliders en physics klaar zijn
        yield return new WaitForEndOfFrame();

        var spawn = GameObject.FindWithTag("PlayerSpawn");
        if (spawn != null)
        {
            // Interpolatie tijdelijk uit
            var oldInterpolation = rb.interpolation;
            rb.interpolation = RigidbodyInterpolation.None;

            // Verplaats speler direct naar spawn
            transform.position = spawn.transform.position;
            // transform.rotation = spawn.transform.rotation; // optioneel

            // Reset physics
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Zet interpolatie terug
            rb.interpolation = oldInterpolation;
        }
    }
}
