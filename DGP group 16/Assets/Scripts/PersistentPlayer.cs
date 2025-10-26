using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer _instance;
    private Rigidbody rb;

    // Hier onthouden we welk spawnpoint moet worden gebruikt
    public static string NextSpawnTag = "";

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
        StartCoroutine(SpawnAtPoint());
    }

    private IEnumerator SpawnAtPoint()
    {
        yield return new WaitForEndOfFrame();

        // Gebruik opgegeven spawn tag of standaard "PlayerSpawn"
        string spawnTag = string.IsNullOrEmpty(NextSpawnTag) ? "PlayerSpawn" : NextSpawnTag;

        var spawn = GameObject.FindWithTag(spawnTag);

        if (spawn != null)
        {
            var oldInterpolation = rb.interpolation;
            rb.interpolation = RigidbodyInterpolation.None;

            transform.position = spawn.transform.position;
            transform.rotation = Quaternion.identity; // altijd naar voren

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.interpolation = oldInterpolation;
        }
        else
        {
            Debug.LogWarning($"Spawnpoint with tag '{spawnTag}' not found. Using default position.");
        }

        // Reset de spawn tag na gebruik
        NextSpawnTag = "";
    }
}