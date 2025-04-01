using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject followCamera;
    public GameObject aimCamera;
    public AimCamera aimCameraSwitch;

    void Start()
    {
        followCamera.SetActive(true);
        aimCamera.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click pressed
        {
            followCamera.SetActive(false);
            aimCamera.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1)) // Right-click released
        {
            followCamera.SetActive(true);
            aimCamera.SetActive(false);
        }
    }
}
