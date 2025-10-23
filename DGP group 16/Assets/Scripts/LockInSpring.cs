using UnityEngine;

public class LockInSpring : MonoBehaviour
{
    private string requiredItemName = "ItemSpring";  // item player must be holding

    [Header("Objects to Change")]
    public GameObject itemToChange1;  // Assign eind veer
    public GameObject itemToChange2;  // Assign eind veer groot
    public Material newMaterial;      // The material to apply to itemToChange1
    public GameObject frontGlass; // Assign front glass
    public GameObject schroef1;
    public GameObject schroef2;
    public GameObject schroef3;
    public GameObject schroef4;


    private Transform playerHoldPoint;
    private const string SpringKey = "SpringPlaced";

    void Start()
    {
        FindPlayerHoldPoint();

        // Check if Spring was already placed before
        //if (PlayerPrefs.GetInt(SpringKey, 0) == 1)
        //{
        //    Renderer rend = itemToChange1.GetComponent<Renderer>();
        //    rend.material = newMaterial;
        //    MeshCollider meshCol = itemToChange2.GetComponent<MeshCollider>();
        //    Renderer rend2 = itemToChange2.GetComponent<Renderer>();
        //    meshCol.enabled = true;
        //    rend2.material = newMaterial;
        //    frontGlass.SetActive(true);
        //    schroef1.SetActive(true);
        //    schroef2.SetActive(true);
        //    schroef3.SetActive(true);
        //    schroef4.SetActive(true);
        //}
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

                // --- Change item 1's material ---
                if (itemToChange1 != null && newMaterial != null)
                {
                    Renderer rend = itemToChange1.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.material = newMaterial;
                        Debug.Log("Changed material on item 1!");
                    }
                    else
                    {
                        Debug.LogWarning("Item 1 has no Renderer component!");
                    }
                }

                // --- Enable mesh collider on item 2 ---
                if (itemToChange2 != null)
                {
                    MeshCollider meshCol = itemToChange2.GetComponent<MeshCollider>();
                    Renderer rend = itemToChange2.GetComponent<Renderer>();
                    if (meshCol != null && rend != null)
                    {
                        meshCol.enabled = true;
                        rend.material = newMaterial;
                        Debug.Log("Enabled MeshCollider on item 2!");
                    }
                    else
                    {
                        Debug.LogWarning("Item 2 has no MeshCollider component!");
                    }
                }

                // Activate front glass and schoef 1-4
                frontGlass.SetActive(true);
                schroef1.SetActive(true);
                schroef2.SetActive(true);
                schroef3.SetActive(true);
                schroef4.SetActive(true);
                PlayerPrefs.SetInt(SpringKey, 1);
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
