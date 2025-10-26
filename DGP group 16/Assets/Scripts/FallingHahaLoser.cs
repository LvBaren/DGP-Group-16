using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class FallingHahaLoser : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform levelEndPoint;
    public Transform cameraTransform;

    [Header("Settings")]
    private float fallSpeed = 2f;

    private Vector3 cameraOffset;
    private bool isFalling = false;

    // Lijst van uitgeschakelde scripts die we later weer aanzetten
    private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerAndStart());
    }

    private IEnumerator WaitForPlayerAndStart()
    {
        // Wacht totdat de speler (met tag "Player") in de scene aanwezig is
        while (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;

            yield return null;
        }

        // Start sequence als alle references bestaan
        if (cameraTransform != null && levelEndPoint != null)
        {
            InitializeFallSequence();
        }
    }

    private void InitializeFallSequence()
    {
        cameraOffset = cameraTransform.position - player.position;

        // Koppel camera aan speler
        cameraTransform.SetParent(player);
        cameraTransform.localPosition = cameraOffset;

        StartCoroutine(FallSequence());
    }

    private IEnumerator FallSequence()
    {
        isFalling = true;

        // Schakel tijdelijk alle scripts op de speler uit (zodat speler geen controle heeft)
        disabledScripts.Clear();
        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();

        foreach (var script in scripts)
        {
            // Laat dit script en de FallingHahaLoser met rust
            if (script != null && script.enabled && script != this)
            {
                script.enabled = false;
                disabledScripts.Add(script);
            }
        }

        // Laat speler vallen tot ondergrens
        while (player.position.y > levelEndPoint.position.y)
        {
            player.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        // Zet alles weer netjes aan
        RestorePlayerScripts();

        // Ontkoppel camera
        if (cameraTransform != null)
            cameraTransform.SetParent(null);

        isFalling = false;
    }

    private void RestorePlayerScripts()
    {
        foreach (var script in disabledScripts)
        {
            if (script != null)
                script.enabled = true;
        }

        disabledScripts.Clear();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Veiligheidscheck: zorg dat camera los is bij nieuwe scene
        if (cameraTransform != null && cameraTransform.parent != null)
            cameraTransform.SetParent(null);

        // Zorg dat scripts opnieuw geactiveerd zijn (extra zekerheid)
        RestorePlayerScripts();
    }
}
