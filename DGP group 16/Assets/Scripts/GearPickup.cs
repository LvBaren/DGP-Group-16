using UnityEngine;

public class GearPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public KeyCode pickupKey = KeyCode.E;
    public float pickupRange = 2f;       

    private Transform player;
    private bool isInRange = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= pickupRange;

        if (isInRange && Input.GetKeyDown(pickupKey))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Debug.Log("Tandwiel opgepakt!");
        Destroy(gameObject); // verwijder het tandwiel
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}