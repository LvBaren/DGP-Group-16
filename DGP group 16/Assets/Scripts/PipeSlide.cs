using UnityEngine;
using System.Collections;

public class PipeSlide : MonoBehaviour
{
    [Header("Slide Settings")]
    public Transform exitPoint;     // waar de speler uitkomt
    public float slideForce = 2f;  // kracht waarmee speler vooruit wordt geduwd
    public float maxSpeed = 14f;    // maximale snelheid tijdens het glijden

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
        // Gravity aan laten voor een realistisch gevoel
        rb.useGravity = true;

        // Schakel tijdelijk andere scripts uit (zoals movement)
        MonoBehaviour[] allScripts = rb.GetComponents<MonoBehaviour>();
        foreach (var script in allScripts)
        {
            if (script.enabled && script != this)
                script.enabled = false;
        }

        // Zolang speler niet bij de uitgang is, duw hem richting de uitgang
        while (Vector3.Distance(rb.position, exitPoint.position) > 1f)
        {
            Vector3 direction = (exitPoint.position - rb.position).normalized;

            if (rb.linearVelocity.magnitude < maxSpeed)
                rb.AddForce(direction * slideForce, ForceMode.Acceleration);

            yield return null;
        }

        // Herstel besturing na glijden
        foreach (var script in allScripts)
        {
            if (script != this)
                script.enabled = true;
        }
    }
}
