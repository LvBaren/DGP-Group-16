using UnityEngine;
using System.Collections;

public class FallingHahaLoser : MonoBehaviour
{
    [Header("References")]
    public Transform player;          
    public Transform levelEndPoint;   
    public Transform cameraTransform; 
    public float fallSpeed = 5f;      

    private Vector3 cameraOffset;     
    private bool isFalling = false;

    void Start()
    {
        if (player != null && cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - player.position;

            cameraTransform.SetParent(player);
            cameraTransform.localPosition = cameraOffset;

            StartCoroutine(FallSequence());
        }
    }

    private IEnumerator FallSequence()
    {
        isFalling = true;

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.enabled && script != this)
                script.enabled = false;
        }

        while (player.position.y > levelEndPoint.position.y)
        {
            player.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        isFalling = false;

        foreach (var script in scripts)
        {
            if (script != this)
                script.enabled = true;
        }

        cameraTransform.SetParent(null);
    }
}
