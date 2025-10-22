using UnityEngine;

public class ProximityReveal : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // De speler (wordt automatisch gevonden als niet ingesteld)
    public GameObject hiddenObject;    // Het object dat zichtbaar moet worden

    [Header("Settings")]
    public float revealDistance = 5f;  // Hoe dichtbij de speler moet zijn

    private bool isVisible = false;

    void Start()
    {
        // Verberg object bij start
        if (hiddenObject != null)
            hiddenObject.SetActive(false);
    }

    void Update()
    {
        if (player == null)
        {
            // Probeer automatisch de speler te vinden
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
            else
                return; // Geen player gevonden wacht tot later
        }

        if (hiddenObject == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= revealDistance && !isVisible)
        {
            // Speler is dichtbij toon object
            hiddenObject.SetActive(true);
            isVisible = true;
        }
        else if (distance > revealDistance && isVisible)
        {
            // Speler is ver weg verberg object
            hiddenObject.SetActive(false);
            isVisible = false;
        }
    }
}
