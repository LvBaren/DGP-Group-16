using UnityEngine;

/// <summary>
/// Bestuurt een rij tandwielen met twee modi:
/// - Shock-modus (stotterende/ping-pong animatie) wanneer het rode tandwiel in de socket zit.
/// - Smooth-modus (constante vloeiende rotatie) wanneer het rode tandwiel uit de socket is.
/// </summary>
public class GearSystemController : MonoBehaviour
{
    [Header("Gears in this system (order can alternate direction)")]
    [Tooltip("Zet hier alle tandwielen in volgorde. Richtingen wisselen automatisch om per index.")]
    public Transform[] gears;

    [Header("Red gear & home socket (optional)")]
    [Tooltip("Verwijs naar het rode tandwiel (PickupItem) dat notifyGearSystem = true heeft.")]
    public PickupItem redGear;
    [Tooltip("Optioneel: de socket waar het rode tandwiel thuishoort (alleen ter referentie).")]
    public GearSocket redGearSocket;

    [Header("Animation Settings")]
    [Tooltip("Snelheid van de schok-animatie (ping-pong).")]
    public float shockRotationSpeed = 2f;     // Hz-achtige factor voor Mathf.Sin(Time.time * speed)
    [Tooltip("Amplitude (in graden) van de schok-animatie (+/-).")]
    public float shockAngleAmplitude = 10f;   // +/- degrees
    [Tooltip("Snelheid in graden/seconde voor de vloeiende, constante rotatie.")]
    public float smoothRotationSpeed = 40f;   // deg/sec

    // true  = schokmodus (rode gear in socket)
    // false = soepel draaien (rode gear eruit)
    private bool isShocking = true;

    // Accumulator voor de vloeiende rotatie zodat we geen sprong krijgen bij mode-wissel.
    private float smoothAngleAccum = 0f;

    void Start()
    {
        // Startmodus bepalen op basis van aanwezigheid van rode gear in de socket
        // (PickupItem zet isMountedInSocket wanneer het in een socket klikt of eruit wordt gehaald).
        if (redGear != null && !redGear.isMountedInSocket)
            isShocking = false; // rode gear is NIET gemonteerd -> soepel
        else
            isShocking = true;  // standaard: schokmodus

        // Beginwaarde netjes binnen 0..360 houden.
        smoothAngleAccum = 0f;
    }

    void Update()
    {
        if (gears == null || gears.Length == 0) return;

        float baseAngle;

        if (isShocking)
        {
            // Ping-pong / schok-animatie
            baseAngle = Mathf.Sin(Time.time * shockRotationSpeed) * shockAngleAmplitude;
        }
        else
        {
            // Constante vloeiende rotatie: accumuleer met deltaTime
            smoothAngleAccum += smoothRotationSpeed * Time.deltaTime;

            // Houd de waarde tussen 0..360 om float-overflow op lange termijn te voorkomen
            if (smoothAngleAccum >= 360f) smoothAngleAccum -= 360f;
            else if (smoothAngleAccum < 0f) smoothAngleAccum += 360f;

            baseAngle = smoothAngleAccum;
        }

        // Pas rotatie toe; alterneer richting per index zodat tandwielen in elkaar grijpen
        for (int i = 0; i < gears.Length; i++)
        {
            var g = gears[i];
            if (!g) continue;

            float a = (i % 2 == 0) ? baseAngle : -baseAngle;
            g.localRotation = Quaternion.Euler(0f, 0f, a);
        }
    }

    // --- Publieke API / aanroepen vanuit PickupItem ----------------------------

    /// <summary>
    /// Wordt aangeroepen door het rode tandwiel wanneer het wordt opgepakt (uit de socket).
    /// Schakel over naar SOEPEL draaien.
    /// </summary>
    public void OnRedGearPickedUp()
    {
        isShocking = false;
    }

    /// <summary>
    /// Wordt aangeroepen door het rode tandwiel wanneer het terug in de socket klikt.
    /// Schakel over naar SCHOK-modus.
    /// </summary>
    public void OnRedGearReturned()
    {
        isShocking = true;
    }

    // --- Backwards compatibility (optioneel) -----------------------------------
    // Als er elders nog StartSystem/StopSystem werd gebruikt, laten we die
    // mappen op respectievelijk schokmodus (aan) en soepel (uit).

    /// <summary>
    /// Compat: behandel "StartSystem" als schokmodus aan.
    /// </summary>
    public void StartSystem()
    {
        isShocking = true;
    }

    /// <summary>
    /// Compat: behandel "StopSystem" als overschakelen naar soepel draaien.
    /// </summary>
    public void StopSystem()
    {
        isShocking = false;
    }
}
