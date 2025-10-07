using UnityEngine;
public class GearRotator : MonoBehaviour
{
    public Vector3 axis = Vector3.forward; // Z-as
    public float speed = 90f;
    public bool spinning = true;
    void Update()
    {
        if (spinning) transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
    }
}
