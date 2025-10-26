using UnityEngine;

public class KijkrichtingSpookje : MonoBehaviour
{
    void Start()
    {
        GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
        if (spawn != null)
        {
            // zet positie + rotatie in één call (mag ook in 2 regels zoals jij deed)
            transform.SetPositionAndRotation(
                spawn.transform.position,
                spawn.transform.rotation
            );
        }
       
    }
}
