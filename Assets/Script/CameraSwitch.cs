using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject followCamera;
    public GameObject aimCamera;
    public GameObject crossHair;
    public AimCamera aimCameraSwitch;
    private Transform playerRotation;
    public Transform bikeRotation;

    public CinemachineFreeLook aimCameracin; // The aim camera
    private float originalCameraX; // Store Cinemachine X rotation
    private float originalCameraY; // Store Cinemachine Y rotation
    public bool isAiming = false;

    void Start()
    {
        playerRotation = GameObject.Find("Player").GetComponent<Transform>();
       

        originalCameraX = aimCameracin.m_XAxis.Value; // Store X-axis rotation
        originalCameraY = aimCameracin.m_YAxis.Value; // Store Y-axis rotation
    }

    void Update()
    {
     /*   if (Input.GetMouseButtonDown(1) && isAiming == false) // Right-click pressed
        {
            isAiming = true;
            followCamera.SetActive(false);
            aimCamera.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1)) // Right-click released
        {
            followCamera.SetActive(true);
            StartCoroutine(SmoothRotateBack()); // Start smooth rotation back
        }*/

      /*  if (!isAiming)
        {
            UpdateCinemachineRotation();
        }*/
    }

    IEnumerator SmoothRotateBack()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = playerRotation.rotation; // Current rotation
        Quaternion targetRotation = Quaternion.Euler(0, bikeRotation.transform.rotation.eulerAngles.y, 0); // Reset to (0,0,0)

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * 5f;
            playerRotation.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            yield return null; // Wait for next frame
        }


        playerRotation.rotation = targetRotation; // Ensure it's fully reset

        aimCamera.SetActive(false);
        isAiming = false;
    }

    void UpdateCinemachineRotation()
    {
        aimCameracin.m_XAxis.Value = bikeRotation.rotation.eulerAngles.y;
    }
}
