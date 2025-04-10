using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;  // Reference to player body for rotation

    private float xRotation = 0f; // Tracks up/down camera rotation

    Vector3 moveDirection;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Lock cursor to the screen center
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate camera up/down (clamping to prevent flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent looking too far up/down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void movement() {

    }


    
}