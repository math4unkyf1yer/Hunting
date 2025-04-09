using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeScript : MonoBehaviour
{
    public int timer = 120;
    private float timeRemaining;
    public TextMeshProUGUI timerText;

    void Start()
    {
        timeRemaining = timer;
    }

    void Update()
    {
        timerText.text = timeRemaining.ToString();
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if (timeRemaining <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
