using UnityEngine;
using System.Collections;

public class PipeSlide : MonoBehaviour
{
    [Header("Slide Settings")]
    public Transform exitPoint;      // waar speler uitkomt
    public float slideForce = 10f;   // kracht waarmee speler vooruit wordt geduwd
    public float maxSpeed = 20f;     // maximum snelheid tijdens glijden
    public float exitBoost = 5f;     // extra snelheid bij het verlaten van de buis

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                StartCoroutine(SlideThroughPipe(rb));
            }
        }
    }

    private IEnumerator SlideThroughPipe(Rigidbody rb)
    {
        // Nieuw systeem: linearDamping ipv drag
        rb.useGravity = true;
        rb.linearDamping = 0.2f; // beetje luchtweerstand

        // Schakel tijdelijk input of andere scripts uit
        MonoBehaviour[] allScripts = rb.GetComponents<MonoBehaviour>();
        foreach (var script in allScripts)
        {
            if (script.enabled && script != this)
                script.enabled = false;
        }

        // Zolang de speler niet bij de uitgang is
        while (Vector3.Distance(rb.position, exitPoint.position) > 1f)
        {
            Vector3 direction = (exitPoint.position - rb.position).normalized;

            // Beperk snelheid
            if (rb.linearVelocity.magnitude < maxSpeed)
                rb.AddForce(direction * slideForce, ForceMode.Acceleration);

            yield return null;
        }

        // Exit boost
        rb.AddForce(exitPoint.forward * exitBoost, ForceMode.VelocityChange);

        // Besturing terug
        foreach (var script in allScripts)
        {
            if (script != this)
                script.enabled = true;
        }
    }
}
