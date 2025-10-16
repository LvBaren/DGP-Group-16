using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform parent, Vector3 localPosition, Vector3 localEuler)
    {
        // stop beweging
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // fysica uit tijdens vasthouden
        rb.isKinematic = true;
        col.enabled = false;

        // aan HoldPoint hangen met gewenste offset/rotatie
        transform.SetParent(parent);
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(localEuler);
    }

    public void Drop(Vector3 worldPos, Vector3 initialImpulse)
    {
        transform.SetParent(null);
        transform.position = worldPos;

        // fysica weer aan
        rb.isKinematic = false;
        col.enabled = true;

        if (initialImpulse != Vector3.zero)
            rb.AddForce(initialImpulse, ForceMode.Impulse);
    }
}
