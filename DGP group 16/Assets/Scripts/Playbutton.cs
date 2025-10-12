using UnityEngine;
using UnityEngine.SceneManagement;

public class Playbutton : MonoBehaviour
{
    void OnMouseDown()
    {
        // This is called when the GameObject is clicked
        SceneManager.LoadScene("Bel");
    }
}
