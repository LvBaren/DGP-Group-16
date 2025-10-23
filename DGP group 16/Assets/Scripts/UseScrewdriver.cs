using UnityEditor;
using UnityEngine;

public class UseScrewdriver : MonoBehaviour
{
    private string requiredItemName = "itemScroevendraaier";  // item player must be holding

    [Header("Objects to Change")]
    public GameObject springBoxPlane;  // De glas van de box van de springveer
    public GameObject springPickupItem;  // Springveer voor pickup
    public GameObject springCoverup; // Springveer not pickupable

    private Transform playerHoldPoint;
    private float moveSpeed = 10f;
    private float yLimit = 10f;

    private const string ScrewdriverKey = "ScrewdriverPlaced";

    void Start()
    {
        FindPlayerHoldPoint();
        if (PlayerPrefs.GetInt(ScrewdriverKey, 0) == 1)
        {
            springCoverup.SetActive(false);
            springPickupItem.SetActive(true);
            springBoxPlane.SetActive(false);
        }
        }

    void FindPlayerHoldPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform hold = player.transform.Find("HoldPoint"); // must match actual name
            if (hold != null)
            {
                playerHoldPoint = hold;
                Debug.Log("Found player hold point!");
            }
            else
            {
                Debug.LogWarning("Player found, but no HoldPoint child!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (playerHoldPoint == null)
            FindPlayerHoldPoint();

        if (playerHoldPoint != null && playerHoldPoint.childCount > 0)
        {
            Transform heldItem = playerHoldPoint.GetChild(0);

            if (heldItem.name == requiredItemName)
            {
                Debug.Log($"Player collided while holding {requiredItemName}!");
                Destroy(heldItem.gameObject);

                // Disable normal spring
                if (springCoverup != null)
                {
                    springCoverup.SetActive(false);
                }

                // Enable spring pickup
                if (springPickupItem != null)
                {
                    springPickupItem.SetActive(true);
                }

                    // Remove glass of box
                    if (springBoxPlane != null)
                {
                    springBoxPlane.SetActive(false);
                }

                PlayerPrefs.SetInt(ScrewdriverKey, 1);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log($"Player collided holding {heldItem.name}, not {requiredItemName}.");
            }
        }
        else
        {
            Debug.Log("Player collided but isn’t holding anything.");
        }
    }
}
