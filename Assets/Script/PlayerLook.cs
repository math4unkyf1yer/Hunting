using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float controllerSensitivity = 100f;
    public Transform playerBody;  // Reference to player body for rotation

    private float xRotation = 0f; // Tracks up/down camera rotation
    private PlayerControlls controls;
    private Vector2 lookInput;

    Vector3 moveDirection;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Lock cursor to the screen center
    }
    private void Awake()
    {
        controls = new PlayerControlls();

        // When the look stick is moved, update input value
        controls.Movement.look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Movement.look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float controllerX = lookInput.x * controllerSensitivity * Time.deltaTime;
        float controllerY = lookInput.y * controllerSensitivity * Time.deltaTime;

        Debug.Log("Look Input: " + lookInput);

        float finalX = mouseX + controllerX;
        float finalY = mouseY + controllerY;

        xRotation -= finalY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * finalX);
    }



}