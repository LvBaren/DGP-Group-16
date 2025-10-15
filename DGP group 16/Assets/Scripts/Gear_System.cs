using UnityEngine;

public class Gear_System : MonoBehaviour
{
    [Header("References")]
    public GameObject[] gears = new GameObject[6];
    public GameObject triggerGear;

    [Header("Rotation Settings")]
    public float targetRotationSpeed = 200f;
    public float acceleration = 50f;

    [Header("Wiggle Settings")]
    public float wiggleAmplitude = 15f;
    public float wiggleSpeed = 3f;

    private float currentSpeed = 0f;
    private bool gearsShouldRotate = false;
    private bool isWiggling = true;

    private Quaternion[] originalRotations; 

    void Start()
    {
        originalRotations = new Quaternion[gears.Length];
        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i] != null)
                originalRotations[i] = gears[i].transform.localRotation;
        }
    }

    void Update()
    {
        if ((triggerGear == null || !triggerGear.activeInHierarchy) && !gearsShouldRotate)
        {
            gearsShouldRotate = true;
            isWiggling = false;
        }

        if (isWiggling)
        {
            WiggleGears();
        }

        if (gearsShouldRotate)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetRotationSpeed, acceleration * Time.deltaTime);
            RotateGears();
        }
    }

    void WiggleGears()
    {
        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i] == null) continue;

            float direction = (i % 2 == 0) ? 1f : -1f;
            float phaseOffset = i * 0.5f;
            float wiggleAngle = Mathf.Sin((Time.time + phaseOffset) * wiggleSpeed * Mathf.PI * 2f) * wiggleAmplitude * direction;

            gears[i].transform.localRotation = originalRotations[i] * Quaternion.Euler(0, 0, wiggleAngle);
        }
    }

    void RotateGears()
    {
        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i] == null) continue;

            float direction = (i % 2 == 0) ? 1f : -1f;
            gears[i].transform.Rotate(Vector3.forward * direction * currentSpeed * Time.deltaTime);
        }
    }
}
