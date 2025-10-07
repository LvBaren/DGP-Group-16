using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class Interact : MonoBehaviour
{
    [Header("Detection")]
    public float pickupRadius = 2.0f;
    public LayerMask pickupMask;
    public Transform detectCenter;

    [Header("References")]
    public PlayerInventory inventory;

    void Awake()
    {
        if (!inventory) inventory = GetComponent<PlayerInventory>();
        if (!detectCenter) detectCenter = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("[Interact] P gedrukt");
            GameObject nearest = FindNearestPickup();
            if (nearest)
            {
                Debug.Log("[Interact] Dichtbij: " + nearest.name);
                inventory.Pickup(nearest);
            }
            else
            {
                Debug.Log("[Interact] Geen oppakbaar object binnen radius.");
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("[Interact] D gedrukt (drop)");
            inventory.DropCurrent();
        }
    }

    GameObject FindNearestPickup()
    {
        Vector3 center = detectCenter.position;
        Collider[] hits = Physics.OverlapSphere(center, pickupRadius, pickupMask, QueryTriggerInteraction.Ignore);
        if (hits.Length == 0) return null;

        Collider best = null;
        float bestDist = float.MaxValue;

        foreach (var c in hits)
        {
            // PickupItem is handig, maar niet verplicht voor deze scan
            float d = (c.ClosestPoint(center) - center).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = c;
            }
        }

        return best ? best.attachedRigidbody ? best.attachedRigidbody.gameObject : best.gameObject : null;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector3 pos = detectCenter ? detectCenter.position : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pos, pickupRadius);
    }
#endif
}
