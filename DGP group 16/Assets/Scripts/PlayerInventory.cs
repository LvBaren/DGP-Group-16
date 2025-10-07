using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("References")]
    public Transform holdPoint;          // Zet hier je HoldPoint (child van Player)
    public GameObject heldItem;

    [Header("Drop settings")]
    public float dropForwardForce = 2f;  // Klein duwtje naar voren bij droppen
    public float dropUpForce = 1f;

    // Interne state om originele waarden te herstellen
    private Rigidbody heldRb;
    private Collider[] heldColliders;
    private int[] originalLayers;

    public bool IsHolding => heldItem != null;

    public void Pickup(GameObject item)
    {
        if (item == null) return;

        // Als we al iets vasthouden: eerst droppen
        if (IsHolding)
            DropCurrent();

        heldItem = item;

        // Rigidbody/Colliders verzamelen
        heldRb = heldItem.GetComponent<Rigidbody>();
        heldColliders = heldItem.GetComponentsInChildren<Collider>(true);

        // Originele layers bewaren
        originalLayers = new int[heldColliders.Length];
        for (int i = 0; i < heldColliders.Length; i++)
            originalLayers[i] = heldColliders[i].gameObject.layer;

        // Fysica “uit” zetten tijdens vasthouden
        if (heldRb != null)
        {
            heldRb.isKinematic = true;
            heldRb.interpolation = RigidbodyInterpolation.None;
            heldRb.linearVelocity = Vector3.zero;
            heldRb.angularVelocity = Vector3.zero;
        }
        foreach (var col in heldColliders)
        {
            // Optioneel: triggeren of naar aparte layer zetten, zodat het niet botst met speler
            col.isTrigger = true;
        }

        // Aan HoldPoint hangen
        heldItem.transform.SetParent(holdPoint, worldPositionStays: false);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        // Als je een specifieke uitlijning wil, kun je hier aanpassen
    }

    public void DropCurrent()
    {
        if (!IsHolding) return;

        // Loskoppelen
        heldItem.transform.SetParent(null);

        // Fysica terug aan
        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.interpolation = RigidbodyInterpolation.Interpolate;

            // Een klein duwtje zodat het voor je op de grond valt
            var player = transform;
            Vector3 force = player.forward * dropForwardForce + Vector3.up * dropUpForce;
            heldRb.AddForce(force, ForceMode.VelocityChange);
        }

        // Colliders herstellen
        if (heldColliders != null)
        {
            for (int i = 0; i < heldColliders.Length; i++)
            {
                heldColliders[i].isTrigger = false;
                if (originalLayers != null && i < originalLayers.Length)
                    heldColliders[i].gameObject.layer = originalLayers[i];
            }
        }

        // Reset state
        heldItem = null;
        heldRb = null;
        heldColliders = null;
        originalLayers = null;
    }
}
