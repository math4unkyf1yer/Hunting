using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCameraAnim : MonoBehaviour
{
    public GameObject cameraAnim1;
    public GameObject cameraAnim2;
    public GameObject cameraAnim3;

    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchWaitCamera());
        StartCoroutine(SwitchWaitCamera2());
    }

   IEnumerator SwitchWaitCamera()
    {
        yield return new WaitForSeconds(1.4f);
        cameraAnim1.SetActive(false);
        cameraAnim2.SetActive(true);
    }
    IEnumerator SwitchWaitCamera2()
    {
        yield return new WaitForSeconds(4f);
        cameraAnim2.SetActive(false);
        cameraAnim3.SetActive(true);
        menu.SetActive(true);
    }
}
