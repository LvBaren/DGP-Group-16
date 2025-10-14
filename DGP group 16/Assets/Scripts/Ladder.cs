using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Ladder Settings")]
    public float climbSpeed = 3f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Test player"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb == null) return;

            // Input uitlezen
            float vertical = Input.GetAxisRaw("Vertical");

            // Klimmen
            if (Mathf.Abs(vertical) > 0.1f)
            {
                rb.useGravity = false;

                // linearVelocity gebruiken in plaats van velocity
                rb.linearVelocity = new Vector3(rb.linearVelocity.x,vertical * climbSpeed,0);
            }
            else
            {
                // Stil hangen op de ladder
                rb.useGravity = false;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Test player"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
}
