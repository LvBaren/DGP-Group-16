using UnityEngine;

public class ResetGame : MonoBehaviour
{
    private const string SpringKey = "SpringPlaced";
    private const string ScrewdriverKey = "ScrewdriverPlaced";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.DeleteKey(SpringKey);
        PlayerPrefs.Save();
        PlayerPrefs.DeleteKey(ScrewdriverKey);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}