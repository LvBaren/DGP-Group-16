using UnityEngine;

/// <summary>
/// Bestuurt een rij tandwielen met twee modi:
/// - Shock-modus (stotterende/ping-pong animatie) wanneer het rode tandwiel in de socket zit.
/// - Smooth-modus (constante vloeiende rotatie) wanneer het rode tandwiel uit de socket is.
/// </summary>
public class GearSystemController : MonoBehaviour
{
    [Header("Gears in this system (order can alternate direction)")]
    public Transform[] gears;

    [Header("Red gear & home socket (optional)")]
    public PickupItem redGear;
    public GearSocket redGearSocket;

    [Header("Animation Settings")]
    public float shockRotationSpeed = 2f;
    public float shockAngleAmplitude = 10f;
    public float smoothRotationSpeed = 40f;

    [Header("Audio")]
    [SerializeField] private AudioClip gearSound;         // Soepele rotatie
    [SerializeField] private AudioClip gearSoundShocking; // Schokmodus
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float minVolumeDistance = 10f;
    [SerializeField] private float maxVolumeDistance = 2f;

    private bool isShocking = true;
    private float smoothAngleAccum = 0f;
    private Quaternion[] startRotations;

    private AudioSource audioSourceSmooth;
    private AudioSource audioSourceShock;
    private Transform player;

    void Start()
    {
        // Sla startrotaties op
        if (gears != null && gears.Length > 0)
        {
            startRotations = new Quaternion[gears.Length];
            for (int i = 0; i < gears.Length; i++)
            {
                startRotations[i] = gears[i] ? gears[i].localRotation : Quaternion.identity;
            }
        }

        // Bepaal beginmodus
        if (redGear != null && !redGear.isMountedInSocket)
            isShocking = false;
        else
            isShocking = true;

        smoothAngleAccum = 0f;

        // Vind speler voor afstandsberekening
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // --- Audio setup ---
        audioSourceSmooth = gameObject.AddComponent<AudioSource>();
        audioSourceSmooth.clip = gearSound;
        audioSourceSmooth.loop = true;
        audioSourceSmooth.playOnAwake = false;
        audioSourceSmooth.spatialBlend = 1f;
        audioSourceSmooth.minDistance = maxVolumeDistance;
        audioSourceSmooth.maxDistance = minVolumeDistance;
        audioSourceSmooth.volume = 0f;

        audioSourceShock = gameObject.AddComponent<AudioSource>();
        audioSourceShock.clip = gearSoundShocking;
        audioSourceShock.loop = true;
        audioSourceShock.playOnAwake = false;
        audioSourceShock.spatialBlend = 1f;
        audioSourceShock.minDistance = maxVolumeDistance;
        audioSourceShock.maxDistance = minVolumeDistance;
        audioSourceShock.volume = 0f;

        // Start meteen het juiste geluid
        if (isShocking)
            audioSourceShock.Play();
        else
            audioSourceSmooth.Play();
    }

    void Update()
    {
        if (gears == null || gears.Length == 0) return;

        // Pas rotatie aan
        float baseAngle;
        if (isShocking)
        {
            baseAngle = Mathf.Sin(Time.time * shockRotationSpeed) * shockAngleAmplitude;
        }
        else
        {
            smoothAngleAccum += smoothRotationSpeed * Time.deltaTime;
            if (smoothAngleAccum >= 360f) smoothAngleAccum -= 360f;
            baseAngle = smoothAngleAccum;
        }

        for (int i = 0; i < gears.Length; i++)
        {
            var g = gears[i];
            if (!g) continue;
            float a = (i % 2 == 0) ? baseAngle : -baseAngle;
            g.localRotation = startRotations[i] * Quaternion.Euler(0f, 0f, a);
        }

        // Pas volume aan op afstand tot speler
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            float vol = Mathf.Clamp01(1 - (distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
            audioSourceSmooth.volume = !isShocking ? vol * maxVolume : 0f;
            audioSourceShock.volume = isShocking ? vol * maxVolume : 0f;
        }
    }

    public void OnRedGearPickedUp()
    {
        if (!isShocking)
            return;

        isShocking = false;
        // Zorg dat smooth geluid start en shock geluid stopt visueel (volume 0)
        if (!audioSourceSmooth.isPlaying) audioSourceSmooth.Play();
    }

    public void OnRedGearReturned()
    {
        if (isShocking)
            return;

        isShocking = true;
        if (!audioSourceShock.isPlaying) audioSourceShock.Play();
    }

    public void StartSystem()
    {
        OnRedGearReturned();
    }

    public void StopSystem()
    {
        OnRedGearPickedUp();
    }
}
