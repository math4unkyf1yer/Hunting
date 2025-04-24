using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public GameObject CameraAnim;
    public GameObject WinUi;

    private void Start()
    {
        StartCoroutine(StartUI());
    }

    IEnumerator StartUI()
    {
        yield return new WaitForSeconds(3f);
        WinUi.SetActive(true);
    }
}
