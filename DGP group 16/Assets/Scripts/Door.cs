using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openHeight = 3f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void Update()
    {
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * openSpeed);
    }

    public void SetOpen(bool state)
    {
        isOpen = state;
    }
}