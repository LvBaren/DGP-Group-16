
using UnityEngine;
using System.Collections;

public class PipeSlide : MonoBehaviour
{
    [Header("Slide Settings")]
    public Transform exitPoint;      // Waar de speler uitkomt
    public float slideForce = 2f;    // Kracht waarmee speler vooruit wordt geduwd
    public float maxSpeed = 14f;     // Maximale snelheid tijdens het glijden
    public float maxSlideTime = 6f;  // Failsafe: maximale tijd dat speler mag glijden

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

        float timer = 0f;

        // Blijf duwen tot bij uitgang, of tot failsafe verloopt
        while (Vector3.Distance(rb.position, exitPoint.position) > 1f && timer < maxSlideTime)
        {
            Vector3 direction = (exitPoint.position - rb.position).normalized;

            if (rb.linearVelocity.magnitude < maxSpeed)
                rb.AddForce(direction * slideForce, ForceMode.Acceleration);

            timer += Time.deltaTime;
            yield return null;
        }

        // Als speler niet op tijd de uitgang haalt zet hem daar neer
        if (Vector3.Distance(rb.position, exitPoint.position) > 1f)
        {
            rb.position = exitPoint.position + Vector3.up * 0.2f; // klein beetje boven de uitgang
        }

        // Herstel alle scripts veilig
        RestorePlayerScripts();

        isSliding = false;
    }

    private void OnDisable()
    {
        RestorePlayerScripts();
    }

    private void OnDestroy()
    {
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
