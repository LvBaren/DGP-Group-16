using UnityEngine;
using UnityEngine.SceneManagement;

/// Eén Player over alle scenes. Voorkomt duplicaten en verplaatst
/// (optioneel) naar een spawnpunt met tag "PlayerSpawn" per scene.
public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer _instance;

    void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (_instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Optioneel: plaats de speler op een spawnpunt als dat bestaat.
        var spawn = GameObject.FindWithTag("PlayerSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            // (optioneel) rotatie ook meenemen:
            // transform.rotation = spawn.transform.rotation;
        }
    }
}
