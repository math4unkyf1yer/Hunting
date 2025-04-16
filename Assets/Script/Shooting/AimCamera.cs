using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class AimCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform bike;
    public float maxAngle = 90f;
    private float lastValidX;
    public Transform PlayerObj;
    public Rigidbody rb;
    public Transform combatLookAt;
    public CinemachineFreeLook freeLookCam;
    public CinemachineFreeLook freeLookCamMouse;

    private string lastControlScheme = "Mouse";
    private float inputCheckDelay = 1f;
    private float inputTimer = 0f;

    //mouse camera
    public GameObject thirdpersonMouse;
    //xbox Camera
    public GameObject thirdPersonXbox;

    public float rotationSpeed;
    public float lookSensitivity = 2f;

    public CameraStyle currentStyle;

    private Vector2 lookInput;
    private PlayerControlls controls;
    public enum CameraStyle
    {
        basic,
        Combat,
        TopDown
    }
    private void Awake()
    {
        controls = new PlayerControlls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        /*  if(currentStyle == CameraStyle.Combat)
          {
              freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
              freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
          }
          else
          {
              freeLookCam.m_XAxis.m_InputAxisName = "RightStickHorizontal";
              freeLookCam.m_YAxis.m_InputAxisName = "RightStickVertical";
          }*/
    }
    void Update()
    {
        var mouseMoved = Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f;
        var gamepadUsed = Gamepad.current != null && (
            Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.01f ||
            Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.01f ||
            Gamepad.current.buttonSouth.wasPressedThisFrame
        );

        if (mouseMoved)
        {
            SetCameraForMouse();
        }
        else if (gamepadUsed)
        {
            SetCameraForGamepad();
        }

        // Your existing camera style rotation logic can stay here...
    }
    private void SetCameraForMouse()
    {
        if (!thirdpersonMouse.activeSelf)
        {
            thirdpersonMouse.SetActive(true);
            thirdPersonXbox.SetActive(false);
            currentStyle = CameraStyle.Combat;
        }
    }

    private void SetCameraForGamepad()
    {
        if (!thirdPersonXbox.activeSelf)
        {
            thirdPersonXbox.SetActive(true);
            thirdpersonMouse.SetActive(false);
            currentStyle = CameraStyle.basic;
        }
    }

    private void FixedUpdate()
    {
        if (player == null || PlayerObj == null || orientation == null)
            return; // Skip update if references are missing

        lookInput = controls.Movement.look.ReadValue<Vector2>();
        if (currentStyle == CameraStyle.Combat)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            Vector3 dirCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirCombatLookAt.normalized;
            PlayerObj.forward = dirCombatLookAt.normalized;
        }
        if (currentStyle == CameraStyle.basic)
        {
            // Align orientation to camera forward on XZ plane
            orientation.forward = Vector3.ProjectOnPlane(freeLookCam.transform.forward, Vector3.up).normalized;

            Vector3 inputDir = new Vector3(lookInput.x, 0, lookInput.y).normalized;
            Vector3 rotatedDir = orientation.transform.TransformDirection(inputDir); // relative to camera

            if (inputDir.magnitude >= 0.1f)
            {
                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, rotatedDir, Time.deltaTime * rotationSpeed);
            }

        }
    }

    private void LateUpdate()
    {
        if (bike == null || freeLookCamMouse == null)
            return;

        // Calculate current direction from bike to camera
        Vector3 camDir = bike.position - freeLookCamMouse.State.FinalPosition;
        camDir.y = 0;
        camDir.Normalize();

        Vector3 bikeForward = bike.forward;
        bikeForward.y = 0;
        bikeForward.Normalize();

        float angle = Vector3.SignedAngle(bikeForward, camDir, Vector3.up);

        if (Mathf.Abs(angle) > maxAngle)
        {
            float clampedAngle = Mathf.Clamp(angle, -maxAngle, maxAngle);
            float delta = clampedAngle - angle;

            // Translate that angle correction into XAxis value correction
            // This is tricky because m_XAxis.Value is in degrees but may wrap, so we use delta time smoothing
            freeLookCamMouse.m_XAxis.Value += delta * Time.deltaTime * 5f; // smooth correction
        }
        else
        {
            lastValidX = freeLookCamMouse.m_XAxis.Value; // store current if valid
        }

    }
}
