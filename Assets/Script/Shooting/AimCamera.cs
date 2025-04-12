using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform PlayerObj;
    public Rigidbody rb;
    public Transform combatLookAt;

    public float rotationSpeed;
    public float lookSensitivity = 2f;

    public CameraStyle currentStyle;

    private PlayerControlls controls;
    private Vector2 lookInput;
    public enum CameraStyle
    {
        basic,
        Combat,
        TopDown
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        if (player == null || PlayerObj == null || orientation == null)
            return; // Skip update if references are missing
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        
         if(currentStyle == CameraStyle.Combat)
        {

             Vector3 dirCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
             orientation.forward = dirCombatLookAt.normalized;
             PlayerObj.forward = dirCombatLookAt.normalized;

            // EXTRA: Allow joystick to rotate the PlayerObj manually
            float joystickX = Input.GetAxis("RightStickHorizontal");
            float joystickY = Input.GetAxis("RightStickVertical");

            Vector3 inputDir = new Vector3(joystickX, 0, joystickY).normalized;

            if (inputDir.magnitude > 0.1f)
            {
                Vector3 rotatedDir = orientation.transform.TransformDirection(inputDir); // make it relative to camera
                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, rotatedDir, Time.deltaTime * rotationSpeed);
            }

        }
    }
}
