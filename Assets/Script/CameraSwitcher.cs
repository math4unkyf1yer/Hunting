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
    [SerializeField]private CinemachineVirtualCamera tpsCam; // Third person cam

    public CinemachineFramingTransposer tpsTransposer;
    public CinemachineFramingTransposer fpsTransposer;
    // Start is called before the first frame update
    void Start()
    {
        action.performed += _ => CameraSwitch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        action.Enable();
    }

    private void OnDisable() {
        action.Disable();
    }

    private void CameraSwitch() {


        Debug.Log("hi");
        if(isFPS) {
            fpsCam.Priority = 0;
            tpsCam.Priority = 1;
            // tpsTransposer.m_RecenterToTargetHeading.m_enabled = true;
        }
        else {
            tpsCam.Priority = 0;
            fpsCam.Priority = 1;

            // fpsTransposer.m_RecenterToTargetHeading.m_enabled = true;

        }
        isFPS = !isFPS;
    }
}
