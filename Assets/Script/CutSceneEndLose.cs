using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneEndLose : MonoBehaviour
{
    public GameObject CameraAnim;
    public GameObject loseUi;

    private void Start()
    {
        StartCoroutine(StartUI());
    }

    IEnumerator StartUI()
    {
        yield return new WaitForSeconds(2f);
        loseUi.SetActive(true);
    }
}
