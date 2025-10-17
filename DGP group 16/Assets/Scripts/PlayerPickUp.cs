using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Hold")]
    public Transform holdPoint;

    [Header("Pickup")]
    public float pickRadius = 1.5f;
    public LayerMask pickupMask = ~0; // Everything for testing

    [Header("Drop")]
    public float dropForward = 0.4f;
    public float dropImpulse = 0f;

    [Header("Sockets")]
    public float socketDetectRadius = 0.7f;
    public LayerMask socketMask = ~0; // set to your socket layer or Everything

    private PickupItem carried;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) TryPickup();
        if (Input.GetKeyDown(KeyCode.Q)) TryDrop();
    }

    void TryPickup()
    {
        if (carried != null || holdPoint == null) return;

        Vector3 center = holdPoint.position;
        Collider[] hits = Physics.OverlapSphere(center, pickRadius, pickupMask);

        PickupItem best = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            var item = h.GetComponent<PickupItem>();
            if (!item) continue;

            float d = (h.transform.position - center).sqrMagnitude;
            if (d < bestDist) { best = item; bestDist = d; }
        }

        if (best == null) return;

        carried = best;
        carried.PickUp(holdPoint);
    }

    void TryDrop()
    {
        if (carried == null) return;

        carried.transform.SetParent(null);

        // Try snap to a nearby socket
        GearSocket socket = FindNearestSocketInFront();
        if (socket != null && socket.CanAccept(carried))
        {
            carried.SnapToSocket(socket);
            carried = null;
            return;
        }

        // Free drop
        Vector3 dropPos = holdPoint.position + holdPoint.forward * dropForward;
        Vector3 impulse = holdPoint.forward * dropImpulse;

        carried.DropFree(dropPos, impulse);
        carried = null;
    }

    GearSocket FindNearestSocketInFront()
    {
        Vector3 origin = holdPoint.position;
        Vector3 probe  = origin + holdPoint.forward * socketDetectRadius * 0.5f;

        Collider[] hits = Physics.OverlapSphere(probe, socketDetectRadius, socketMask);

        GearSocket best = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            var s = h.GetComponent<GearSocket>();
            if (!s) continue;

            float d = (h.transform.position - origin).sqrMagnitude;
            if (d < bestDist) { best = s; bestDist = d; }
        }
        return best;
    }

    void OnDrawGizmosSelected()
    {
        if (!holdPoint) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holdPoint.position, pickRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(holdPoint.position + holdPoint.forward * socketDetectRadius * 0.5f, socketDetectRadius);
    }
}
