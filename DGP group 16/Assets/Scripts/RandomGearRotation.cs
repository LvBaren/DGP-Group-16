using UnityEngine;

public class RandomGearRotation : MonoBehaviour
{
    [Header("Instellingen")]
    public Vector2 speedRangeDegPerSec = new Vector2(20f, 60f); // min/max snelheid in graden/sec
    public bool randomDirection = true;                          // soms linksom, soms rechtsom
    public Vector3 axis = Vector3.forward;                       // Z-as (bovenaanzicht)

    float _speed; // gekozen snelheid voor dit tandwiel

    void Start()
    {
        // Kies een willekeurige snelheid tussen min en max
        _speed = Random.Range(speedRangeDegPerSec.x, speedRangeDegPerSec.y);

        // Willekeurig linksom/rechtsom
        if (randomDirection && Random.value < 0.5f)
            _speed = -_speed;
    }

    void Update()
    {
        // Draai elke frame een stukje
        transform.Rotate(axis * (_speed * Time.deltaTime), Space.Self);
    }
}
