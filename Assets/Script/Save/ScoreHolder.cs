using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHolder : MonoBehaviour
{
    private SaveLoad saveLoadScript;
    public TextMeshProUGUI[] summerText;
    public TextMeshProUGUI[] winterText;
    public TextMeshProUGUI[] fallText;
    private void Start()
    {
        saveLoadScript = GameObject.Find("HighScoreHolder").GetComponent<SaveLoad>();
        for (int i = 0; i < summerText.Length; i++)
        {
            summerText[i].text = saveLoadScript.summerHighScore[i].ToString();
        }
        for (int i = 0; i < winterText.Length; i++)
        {
            winterText[i].text = saveLoadScript.winterHighScore[i].ToString();
        }
        for (int i = 0; i < fallText.Length; i++)
        {
            fallText[i].text = saveLoadScript.fallHighScore[i].ToString();
        }
    }


}
