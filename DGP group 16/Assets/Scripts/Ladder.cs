using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Ladder Settings")]
    public float climbSpeed = 3f;
    public float topExitBuffer = 0.5f;     // afstand boven de ladder waar speler niet meer klimt
    public float bottomExitBuffer = 0.3f;  // afstand onderaan voor soepele start/exit
    public float stepOffDownForce = -1.5f; // kleine ‘duw’ omlaag bij verlaten onderkant

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

        float vertical = Input.GetAxisRaw("Vertical"); // W/S, pijltjes of stick
        Vector3 rbPos = rb.position;

        // Bepaal bruikbare klimzone van de ladder
        float ladderTop = ladderCollider.bounds.max.y - topExitBuffer;
        float ladderBottom = ladderCollider.bounds.min.y + bottomExitBuffer;

        bool atTop = rbPos.y >= ladderTop;
        bool atBottom = rbPos.y <= ladderBottom;

        // === KLIMMEN ===
        if (Mathf.Abs(vertical) > 0.1f)
        {
            // KLIM OMHOOG
            if (vertical > 0f)
            {
                if (!atTop)
                {
                    rb.useGravity = false;
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, vertical * climbSpeed, 0f);
                }
                else
                {
                    // Bovenkant bereikt: loslaten/uitstappen
                    rb.linearVelocity = Vector3.zero;
                    rb.useGravity = true; // speler stapt vanzelf uit bovenaan
                }
            }
            // KLIM OMLAAG
            else // vertical < 0
            {
                if (!atBottom)
                {
                    rb.useGravity = false;
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, vertical * climbSpeed, 0f);
                }
                else
                {
                    // Onderrand: laat de speler van de ladder af “glijden”
                    rb.useGravity = true;
                    // kleine impuls/duw naar beneden voor natuurlijk uitstappen
                    var v = rb.linearVelocity;
                    v.y = Mathf.Min(v.y, stepOffDownForce);
                    rb.linearVelocity = v;
                }
            }
        }
        else
        {
            // Geen input: stil hangen aan de ladder (geen zakkende gravity)
            // Alleen als we NIET aan de randen staan — aan de randen laten we default gedrag toe
            if (!atTop && !atBottom)
            {
                rb.useGravity = false;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
            }
            else
            {
                // aan rand: niet geforceerd hangen zodat uitstappen natuurlijk blijft
                rb.useGravity = true;
            }
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
                // laat horizontale/z-velocity intact; zet alleen verticale stil als hij ‘hing’
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Min(0f, rb.linearVelocity.y), rb.linearVelocity.z);
            }
        }
    }
}
