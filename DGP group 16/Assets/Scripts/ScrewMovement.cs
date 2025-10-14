using UnityEngine;

public class ScrewMovement : MonoBehaviour
{
    public float rotationSpeed = 200f;     // snelheid van draaien
    public float moveSpeed = 0.5f;         // snelheid van op/af bewegen
    public float moveAmplitude = 0.5f;     // hoe ver de schroef beweegt omhoog/omlaag
    public bool rotateAroundY = true;      // als de schroef rechtop staat: Y-as, anders Z-as

    private Vector3 startPosition;
    private float randomOffset;

    void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 100f); // willekeurige fase per schroef

        // Variatie per schroef voor wat dynamiek
        rotationSpeed *= Random.Range(0.8f, 1.2f);
        moveSpeed *= Random.Range(0.8f, 1.2f);
        moveAmplitude *= Random.Range(0.9f, 1.3f);
    }

    void Update()
    {
        // 🔄 Rotatie (zoals een echte schroef)
        if (rotateAroundY)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        else
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // ⬆️⬇️ Verticale beweging (sinusbeweging)
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed + randomOffset) * moveAmplitude;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
