using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Vasthouden")]
    public Transform holdPoint;              // sleep je HoldPoint hierheen
    public Vector3 holdOffset = Vector3.zero;// bv. (0, 0, 0)
    public Vector3 holdRotation = Vector3.zero; // bv. (0, 90, 0) om 'm te draaien

    [Header("Oppak-instellingen")]
    public float pickRadius = 0.9f;          // hoe ver je reikt
    public LayerMask pickupMask;             // zet op je 'Pickup' layer (of Everything)

    [Header("Droppen")]
    public float dropForward = 0.4f;         // zet 'm net voor je neer
    public float dropImpulse = 0.0f;         // mini-throw (0 = geen)

    private PickupItem carried;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TryPickup();
        if (Input.GetKeyDown(KeyCode.D)) Drop();
    }

    void TryPickup()
    {
        if (carried != null || holdPoint == null) return;

        // Zoek dichtstbijzijnde oppakbare collider
        Collider[] hits = Physics.OverlapSphere(holdPoint.position, pickRadius, pickupMask);
        PickupItem best = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            var item = h.GetComponent<PickupItem>();
            if (item == null) continue;

            float d = Vector3.SqrMagnitude(h.transform.position - holdPoint.position);
            if (d < bestDist)
            {
                best = item;
                bestDist = d;
            }
        }

        if (best == null) return;

        carried = best;
        carried.PickUp(holdPoint, holdOffset, holdRotation);
    }

    public void Drop()
    {
        if (carried == null) return;

        // positie net voor het spookje
        Vector3 dropPos = holdPoint.position + holdPoint.forward * dropForward;

        // kleine impuls naar voren (optioneel)
        Vector3 impulse = holdPoint.forward * dropImpulse;

        carried.Drop(dropPos, impulse);
        carried = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (holdPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holdPoint.position, pickRadius);
    }
}
