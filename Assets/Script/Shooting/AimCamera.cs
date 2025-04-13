using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform PlayerObj;
    public Rigidbody rb;
    public Transform combatLookAt;
    public CinemachineFreeLook freeLookCam;


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
        if(currentStyle == CameraStyle.basic)
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
}
