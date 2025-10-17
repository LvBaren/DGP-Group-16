using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GearSocket : MonoBehaviour
{
    [Tooltip("Exact attach point. Leave empty to use this object transform.")]
    public Transform mountPoint;

    [Tooltip("If true, only the red gear (PickupItem.notifyGearSystem) is accepted.")]
    public bool acceptOnlyRedGear = true;

    public UnityEvent onItemSnapped;

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    public bool CanAccept(PickupItem item)
    {
        if (item == null) return false;
        if (acceptOnlyRedGear && !item.notifyGearSystem) return false;
        return true;
    }

    public void OnItemSnapped(PickupItem item)
    {
        onItemSnapped?.Invoke();
    }
}
