using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Ladder Settings")]
    public float climbSpeed = 3f;
    public float topExitBuffer = 0.5f; // afstand boven de ladder waar speler niet meer klimt
    public float bottomExitBuffer = 0.3f; // afstand onderaan voor soepele start

    private Collider ladderCollider;

    private void Start()
    {
        ladderCollider = GetComponent<Collider>();
        if (ladderCollider == null)
        {
            Debug.LogWarning("Ladder heeft geen collider! Voeg een BoxCollider toe met 'Is Trigger' aan.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 rbPos = rb.position;

        // Grenzen van de ladder berekenen
        float ladderTop = ladderCollider.bounds.max.y - topExitBuffer;
        float ladderBottom = ladderCollider.bounds.min.y + bottomExitBuffer;

        // Check of speler binnen de klimzone zit
        bool atTop = rbPos.y >= ladderTop;
        bool atBottom = rbPos.y <= ladderBottom;

        if (Mathf.Abs(vertical) > 0.1f && !atTop && !atBottom)
        {
            // Klimmen
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, vertical * climbSpeed, 0);
        }
        else if (atTop)
        {
            // Stop klimmen bovenaan ladder
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = true;
        }
        else
        {
            // Stil hangen op de ladder
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.useGravity = true;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            }
        }
    }
}
