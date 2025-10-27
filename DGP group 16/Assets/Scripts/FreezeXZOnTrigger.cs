using UnityEngine;

public class FreezeXZOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check of het de speler is
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Bevries X en Z positie, maar laat rotatie vrij
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }
}
