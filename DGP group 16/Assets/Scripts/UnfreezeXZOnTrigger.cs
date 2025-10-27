using UnityEngine;

public class UnfreezeXZOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Zet de constraints terug naar alleen rotatie bevriezen
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }
}
