//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class NextLevelPlane : MonoBehaviour
//{
//    [Header("Scene to load on collision")]
//    public string sceneName; // Set this in the Inspector

//    [Header("Optional: Custom spawn point tag or name for next scene")]
//    public string nextSpawnTag = ""; // leave empty to use default "PlayerSpawn"

//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            LoadNextScene();
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            LoadNextScene();
//        }
//    }

//    private void LoadNextScene()
//    {
//        // Sla het gewenste spawnpoint op, zodat de PersistentPlayer het weet
//        PersistentPlayer.NextSpawnTag = nextSpawnTag;

//        SceneManager.LoadScene(sceneName);
//    }
//}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPlane : MonoBehaviour
{
    [Header("Scene to load on collision")]
    public string sceneName; // Set this in the Inspector

    [Header("Optional: Custom spawn point tag or name for next scene")]
    public string nextSpawnTag = ""; // leave empty to use default "PlayerSpawn"

    [SerializeField] Animator transition;

    public float transitionTime = 0.5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        // Sla het gewenste spawnpoint op, zodat de PersistentPlayer het weet
        PersistentPlayer.NextSpawnTag = nextSpawnTag;
        SceneManager.LoadScene(sceneName);
        

        

        transition.SetTrigger("Start");
    }
}