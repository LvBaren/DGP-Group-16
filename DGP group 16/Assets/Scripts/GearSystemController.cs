using UnityEngine;

public class GearSystemController : MonoBehaviour
{
    [Header("Gears in this system (order can alternate direction)")]
    public Transform[] gears;

    [Header("Red gear & home socket (optional)")]
    public PickupItem redGear;        // set to your GearTrigger (notifyGearSystem = true)
    public GearSocket redGearSocket;  // socket where red gear belongs

    [Header("Animation")]
    public float rotationSpeed = 2f;   // speed of ping-pong
    public float angleAmplitude = 10f; // +/- degrees
    public bool startsActive = true;

    private bool systemActive;

    void Start()
    {
        systemActive = startsActive;

        // If we rely on the red gear presence:
        if (redGear != null && !redGear.isMountedInSocket)
            systemActive = false;
    }

    void Update()
    {
        if (!systemActive) return;

        float angle = Mathf.Sin(Time.time * rotationSpeed) * angleAmplitude;

        for (int i = 0; i < gears.Length; i++)
        {
            var g = gears[i];
            if (!g) continue;

            float a = (i % 2 == 0) ? angle : -angle; // alternate directions like real gears
            g.localRotation = Quaternion.Euler(0f, 0f, a);
        }
    }

    public void StopSystem()  { systemActive = false; }
    public void StartSystem() { systemActive = true;  }

    // Called by PickupItem on the red gear:
    public void OnRedGearPickedUp() { StopSystem(); }
    public void OnRedGearReturned() { StartSystem(); }
}
