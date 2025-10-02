using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    public float moveSpeed = 7f;
    public CameraPresetManager presetManager;

    // Update is called once per frame
    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            inputVector.y = +1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            inputVector.y = -1;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputVector.x = -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputVector.x = +1;
        }
        inputVector = inputVector.normalized;

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDirection * Time.deltaTime * moveSpeed;

        if (Input.GetKey(KeyCode.C))
        {
            presetManager.SwitchToCamera(0);
        }
        else if (Input.GetKey(KeyCode.V))
        {
            presetManager.SwitchToCamera(1);
        }
        else if ((Input.GetKey(KeyCode.B))) {
            presetManager.SwitchToCamera(2);
        }
        else if ((Input.GetKey(KeyCode.N)))
        {
            presetManager.SwitchToCamera(3);
        }
    }
}
