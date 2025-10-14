using UnityEngine;

[AddComponentMenu("Scripts/Quests/SpinningSymbol")]
public class Rotating_gear : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 100);
    }
}
