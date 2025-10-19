using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Door))]
public class DoorGateController : MonoBehaviour
{
    [Header("References (vereist)")]
    public GameObject nextLevelPlane;   // wordt door dit script aan/uit gezet
    public Transform socketPoint;       // bv. jouw object "Socket" of "Plek waar tandwiel moet komen"

    [Header("Zoeken / Detectie")]
    [Tooltip("Alleen tandwielen binnen deze straal rond de socket worden overwogen.")]
    public float searchRadiusAroundSocket = 2f;

    [Tooltip("Maximale afstand tot de socket om te tellen als 'in socket'.")]
    public float socketSnapTolerance = 0.12f;

    [Header("Deur/open-gedrag")]
    [Tooltip("Wacht tot de deur fysiek boven is (i.v.m. Lerp) voor we de plane tonen.")]
    public bool waitUntilFullyOpen = true;

    [Tooltip("Hoe dicht bij de open-positie de deur moet zijn om als 'open' te tellen.")]
    public float openTolerance = 0.03f;

    // --- intern ---
    private Door door;
    private PickupItem gear;            // het gevonden tandwiel
    private Vector3 expectedOpenPos;
    private bool planeShown;

    void Awake()
    {
        door = GetComponent<Door>();

        // Volledig via code: plane uit bij start
        if (nextLevelPlane != null) nextLevelPlane.SetActive(false);
        planeShown = false;
    }

    void Start()
    {
        if (socketPoint == null)
        {
            Debug.LogError("[DoorGateController] Socket Point is NIET ingesteld. Sleep je socket Transform in dit veld.");
        }

        expectedOpenPos = transform.position + Vector3.up * door.openHeight;
        Debug.Log($"[DoorGateController] Open target = {expectedOpenPos}");
    }

    void Update()
    {
        if (socketPoint == null) return;

        // 1) Zoek automatisch het dichtstbijzijnde PickupItem bij de socket (eenmalig vastleggen)
        if (gear == null)
        {
            var all = FindObjectsOfType<PickupItem>();
            if (all != null && all.Length > 0)
            {
                gear = all
                    .Where(pi => pi != null &&
                                 Vector3.Distance(pi.transform.position, socketPoint.position) <= searchRadiusAroundSocket)
                    .OrderBy(pi => Vector3.Distance(pi.transform.position, socketPoint.position))
                    .FirstOrDefault();

                if (gear != null)
                    Debug.Log($"[DoorGateController] Tandwiel gevonden bij socket: {gear.name}");
            }
        }

        if (gear == null) return;

        // 2) Bepaal of het tandwiel 'in de socket' zit
        bool mountedFlag = gear.isMountedInSocket; // jouw pickup/snap code zou dit op true zetten
        float distToSocket = Vector3.Distance(gear.transform.position, socketPoint.position);
        bool nearSocket = distToSocket <= socketSnapTolerance;

        bool gearInSocket = mountedFlag || nearSocket;

        // 3) Deur openen/sluiten
        door.SetOpen(gearInSocket);

        // 4) Plane tonen wanneer deur open (en evt. volledig open)
        if (!planeShown && nextLevelPlane != null && gearInSocket)
        {
            if (waitUntilFullyOpen)
            {
                float dOpen = Vector3.Distance(transform.position, expectedOpenPos);
                if (dOpen <= openTolerance)
                {
                    nextLevelPlane.SetActive(true);
                    planeShown = true;
                    Debug.Log("[DoorGateController] Plane geactiveerd (deur volledig open).");
                }
                else
                {
                    // debug zodat je ziet waarom hij nog wacht
                    Debug.Log($"[DoorGateController] Wacht op volledig open. Afstand={dOpen:F3} (tol={openTolerance})");
                }
            }
            else
            {
                nextLevelPlane.SetActive(true);
                planeShown = true;
                Debug.Log("[DoorGateController] Plane geactiveerd (deur mag al open).");
            }
        }

        // Optioneel: als tandwiel uit socket gaat, plane weer uit (haal weg als je 'm permanent aan wilt laten)
        if (planeShown && !gearInSocket)
        {
            nextLevelPlane.SetActive(false);
            planeShown = false;
            Debug.Log("[DoorGateController] Plane gedeactiveerd (tandwiel niet meer in socket).");
        }
    }
}
