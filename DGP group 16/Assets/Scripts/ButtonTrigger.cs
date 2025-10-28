using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [Header("References")]
    public Door linkedDoor;             // De deur die deze knop bedient
    public GameObject nextLevelPlane;   // Wordt geactiveerd als deur opent
    public Transform buttonTop;         // Het bewegende deel van de knop
    public ButtonManager buttonManager; // Centrale manager

    [Header("Settings")]
    public float pressDepth = 0.1f;
    public float pressSpeed = 5f;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;     // Renderer van de knop
    public Material defaultMaterial;    // Normale kleur
    public Material pressedMaterial;    // Kleur bij indrukken

    private Vector3 unpressedPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (buttonTop != null)
        {
            unpressedPosition = buttonTop.localPosition;
            pressedPosition = unpressedPosition - new Vector3(0, pressDepth, 0);
        }

        // Zorg dat de NextLevelPlane standaard uit staat
        if (nextLevelPlane != null)
            nextLevelPlane.SetActive(false);

        // Stel standaardmateriaal in
        if (buttonRenderer != null && defaultMaterial != null)
            buttonRenderer.material = defaultMaterial;
    }

    void Update()
    {
        if (buttonTop != null)
        {
            Vector3 targetPos = isPressed ? pressedPosition : unpressedPosition;
            buttonTop.localPosition = Vector3.Lerp(buttonTop.localPosition, targetPos, Time.deltaTime * pressSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            audioManager.PlaySFX(audioManager.buttonPress);
            audioManager.PlaySFX(audioManager.doorOpen);
            // Visuele feedback
            if (buttonRenderer != null && pressedMaterial != null)
                buttonRenderer.material = pressedMaterial;

            // Meld aan de manager dat deze knop is geactiveerd
            if (buttonManager != null)
                buttonManager.OnButtonPressed(this);

            // Activeer de NextLevelPlane bij deze deur
            if (nextLevelPlane != null)
                nextLevelPlane.SetActive(true);
        }
    }

    public void ResetButton()
    {
        isPressed = false;
        audioManager.PlaySFX(audioManager.buttonRelease);
        audioManager.PlaySFX(audioManager.doorClose);
        // Deactiveer de NextLevelPlane als deze knop wordt uitgeschakeld
        if (nextLevelPlane != null)
            nextLevelPlane.SetActive(false);

        // Zet het materiaal terug naar de standaardkleur
        if (buttonRenderer != null && defaultMaterial != null)
            buttonRenderer.material = defaultMaterial;
    }
}
