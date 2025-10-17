using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Vasthouden")]
    public Transform holdPoint;                          // sleep je HoldPoint hierheen
    public Vector3 holdOffset   = new Vector3(0f, 0.2f, 0.6f);
    public Vector3 holdRotation = new Vector3(0f, 90f, 0f);

    [Header("Oppakken")]
    public float pickRadius = 0.9f;
    public LayerMask pickupMask;                         // zet tijdelijk op Everything als test

    [Header("Droppen")]
    public float dropForward = 0.4f;
    public float dropImpulse = 0f;

    private PickupItem carried;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TryPickup();
        if (Input.GetKeyDown(KeyCode.D)) Drop();
    }

    private void TryPickup()
    {
        if (carried != null || holdPoint == null) return;

        // Zoek dichtstbijzijnde oppakbare collider rond het holdPoint
        Collider[] hits = Physics.OverlapSphere(holdPoint.position, pickRadius, pickupMask);
        PickupItem best = null; float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            var item = h.GetComponent<PickupItem>();
            if (!item) continue;

            float d = (h.transform.position - holdPoint.position).sqrMagnitude;
            if (d < bestDist) { best = item; bestDist = d; }
        }

        if (best == null) return;

        carried = best;
        carried.PickUp(holdPoint, holdOffset, holdRotation); // parent aan persistent Player
    }

    public void Drop()
    {
        if (carried == null) return;

        // 1) EERST loskoppelen zodat het een root-obj wordt
        carried.transform.SetParent(null);

        // 2) DAN naar de actieve scene verplaatsen (lost "not a root in a scene" op)
        Scene active = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(carried.gameObject, active);

        // 3) Drop-positie/impuls bepalen
        Vector3 dropPos = holdPoint.position + holdPoint.forward * dropForward;
        Vector3 impulse = holdPoint.forward * dropImpulse;

        // 4) Physics weer aan en klaar
        carried.Drop(dropPos, impulse);

        // 5) Player draagt niets meer
        carried = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!holdPoint) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holdPoint.position, pickRadius);
    }
}
