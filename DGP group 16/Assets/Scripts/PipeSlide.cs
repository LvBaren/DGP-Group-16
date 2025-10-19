using UnityEngine;
using System.Collections;

public class PipeSlide : MonoBehaviour
{
    [Header("Slide Settings")]
    public Transform exitPoint;      // Waar de speler uitkomt
    public float slideForce = 2f;    // Kracht waarmee speler vooruit wordt geduwd
    public float maxSpeed = 14f;     // Maximale snelheid tijdens het glijden

    private bool isSliding = false;
    private Rigidbody currentRb;
    private MonoBehaviour[] disabledScripts;

    private void OnTriggerEnter(Collider other)
    {
        if (isSliding) return; // voorkom dubbele triggers

        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                currentRb = rb;
                StartCoroutine(SlideThroughPipe(rb));
            }
        }
    }

    private IEnumerator SlideThroughPipe(Rigidbody rb)
    {
        isSliding = true;
        rb.useGravity = true;

        // Schakel tijdelijk movement-scripts uit
        disabledScripts = rb.GetComponents<MonoBehaviour>();
        foreach (var script in disabledScripts)
        {
            if (script.enabled && script != this)
                script.enabled = false;
        }

        // Blijf duwen tot bij uitgang
        while (Vector3.Distance(rb.position, exitPoint.position) > 1f)
        {
            Vector3 direction = (exitPoint.position - rb.position).normalized;

            if (rb.linearVelocity.magnitude < maxSpeed)
                rb.AddForce(direction * slideForce, ForceMode.Acceleration);

            yield return null;
        }

        // ? Herstel alle scripts veilig
        RestorePlayerScripts();

        isSliding = false;
    }

    private void OnDisable()
    {
        // ? Als dit script verdwijnt (bijv. scene load), herstel de player ook
        RestorePlayerScripts();
    }

    private void OnDestroy()
    {
        // ? Extra failsafe voor zekerheid
        RestorePlayerScripts();
    }

    private void RestorePlayerScripts()
    {
        if (disabledScripts != null)
        {
            foreach (var script in disabledScripts)
            {
                if (script != null)
                    script.enabled = true;
            }

            disabledScripts = null;
        }
    }
}
