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

    public CameraStyle currentStyle;
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
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if(currentStyle == CameraStyle.basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirCombatLookAt.normalized;
            PlayerObj.forward = dirCombatLookAt.normalized;

        }
    }
}
