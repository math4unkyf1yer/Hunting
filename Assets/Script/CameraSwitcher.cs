using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public InputAction action;
    private bool isFPS;
    private bool isTPS;

    [SerializeField]private CinemachineVirtualCamera fpsCam; //First person cam
    [SerializeField]private CinemachineFreeLook tpsCam; // Third person cam

    public CinemachineFramingTransposer tpsTransposer;
    public CinemachineFramingTransposer fpsTransposer;
    // Start is called before the first frame update
    void Start()
    {
        //action.performed += _ => CameraSwitch();
    }

    // Update is called once per frame
    void Update()
    {
        CameraSwitch();
    }

   /* private void OnEnable() {
        action.Enable();
    }

    private void OnDisable() {
        action.Disable();
    }*/

    private void CameraSwitch() {

        if (Input.GetMouseButtonDown(1)) // Right-click pressed
        {
            fpsCam.Priority = 1;
            tpsCam.Priority = 0;
        }
        else if (Input.GetMouseButtonUp(1)) // Right-click released
        {
            tpsCam.Priority = 1;
            fpsCam.Priority = 0;
        }

        isFPS = !isFPS;
    }
}
