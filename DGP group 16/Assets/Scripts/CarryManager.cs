using UnityEngine;

public class CarryManager : MonoBehaviour
{
    public static CarryManager Instance { get; private set; }

    // Het object dat we meenemen (als we iets dragen)
    public GameObject carriedItem;       

    // Waar moet het in de nieuwe scene aan vast (wordt door PlayerPickup doorgegeven)
    public Transform currentHoldPoint;

    // De gewenste lokale offset en rotatie t.o.v. HoldPoint
    public Vector3 savedLocalPos;
    public Vector3 savedLocalEuler;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
