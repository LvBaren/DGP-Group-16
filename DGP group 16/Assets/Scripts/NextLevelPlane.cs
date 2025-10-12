using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPlane : MonoBehaviour
{
    [Header("Scene to load on collision")]
    public string sceneName; // Set this in the Inspector

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // Optional: if the collider is a Trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
