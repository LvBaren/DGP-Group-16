using UnityEngine;

public class HahaLoser : MonoBehaviour
{
    public Transform player;   
    public Vector3 offset;     

    void Start()
    {
        if (player != null)
        {
            transform.SetParent(player);
            transform.localPosition = offset;
        }
    }
}
