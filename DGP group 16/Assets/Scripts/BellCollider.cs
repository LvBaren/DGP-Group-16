using UnityEngine;

public class BellCollider : MonoBehaviour
{
    private Bell parentBell;

    private void Awake()
    {
        parentBell = GetComponentInParent<Bell>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parentBell != null)
        {
            parentBell.OnBellHit(collision);
        }
    }
}
