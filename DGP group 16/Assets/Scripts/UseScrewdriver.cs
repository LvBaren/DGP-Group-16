using UnityEditor;
using UnityEngine;

public class UseScrewdriver : MonoBehaviour
{
    private string requiredItemName = "itemScroevendraaier";  // item player must be holding

    [Header("Objects to Change")]
    public GameObject itemToChange1;  // Assign in Inspector
    public GameObject itemToChange2;  // Assign in Inspector
    public MonoBehaviour scriptNameToEnable;  // The name of the script on itemToChange2

    private Transform playerHoldPoint;
    private float moveSpeed = 10f;
    private float yLimit = 10f;

    void Start()
    {
        FindPlayerHoldPoint();
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

                // Enable pickup
                if (scriptNameToEnable != null)
                {
                    scriptNameToEnable.enabled = true;
                }

                // fly out spawnerbox
                if (itemToChange1 != null)
                {
                    MeshCollider meshCol = itemToChange1.GetComponent<MeshCollider>();
                    if (meshCol != null)
                    {
                        meshCol.enabled = false;
                        itemToChange1.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                        if (itemToChange1.transform.position.y > yLimit)
                        {
                            Destroy(itemToChange1.gameObject);
                        }
                    }
                    
                    // --- Enable another script on item 2 ---
                    if (scriptNameToEnable != null)
                    {
                       
                    }
                }

                // Optional: disable this trigger after activation
                // gameObject.SetActive(false);
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
