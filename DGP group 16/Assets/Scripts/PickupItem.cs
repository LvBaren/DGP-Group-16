using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    /// <summary>
    /// Zet het item vast aan het holdPoint van de speler.
    /// Player is persistent, dus het item reist als child mee tussen scenes.
    /// </summary>
    public void PickUp(Transform parent, Vector3 localPos, Vector3 localEuler)
    {
        // Altijd eerst de beweging stoppen...
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // ...dán pas kinematic zetten (anders krijg je de warning).
        rb.isKinematic = true;
        col.enabled = false;

        // Als child aan het holdPoint hangen.
        transform.SetParent(parent);
        transform.localPosition = localPos;
        transform.localRotation = Quaternion.Euler(localEuler);
    }

    /// <summary>
    /// Laat het item los op worldPos en geef optioneel een duwtje.
    /// </summary>
    public void Drop(Vector3 worldPos, Vector3 impulse)
    {
        transform.position = worldPos;

        rb.isKinematic = false;
        col.enabled = true;

        if (impulse.sqrMagnitude > 0f)
            rb.AddForce(impulse, ForceMode.Impulse);
    }
}
