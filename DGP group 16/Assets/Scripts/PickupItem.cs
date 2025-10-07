using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [HideInInspector] public bool isHeld;

    void OnTransformParentChanged()
    {
        // Als we een parent hebben genaamd "HoldPoint" ergens omhoog, beschouwen we 'm als held
        Transform t = transform;
        isHeld = false;
        while (t != null)
        {
            if (t.name == "HoldPoint") { isHeld = true; break; }
            t = t.parent;
        }
    }
}
