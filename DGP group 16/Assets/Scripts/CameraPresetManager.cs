using UnityEngine;

public class CameraPresetManager : MonoBehaviour
{
    public Camera[] cameras;
    private int currentIndex = 0;

    private void Start()
    {
        SwitchToCamera(0);
    }

    public void SwitchToCamera(int index)
    {
        if (index < 0 || index >= cameras.Length) { return; }
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        cameras[index].gameObject.SetActive(true);
        currentIndex = index;
    }
}
