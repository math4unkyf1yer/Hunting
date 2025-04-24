using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TimeScript : MonoBehaviour
{
    public int timer = 120;
    private float timeRemaining;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deerDefeated;
    public Transform[] ASDJSAD;

    private int minutes;
    private int seconds;
    // Counts how many deer are killed
    public int enemyCount = 0;
    public int amountTodefeat;

    void Start()
    {
        timeRemaining = timer;
    }

    void Update()
    {
        deerDefeated.text = enemyCount + "/" + amountTodefeat.ToString();
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if (timeRemaining <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            timeRemaining = 0;
            if(enemyCount >= amountTodefeat) {
                SceneManager.LoadScene("WinScreen");
            }
            else
                SceneManager.LoadScene("LoseScreen");
            
        }

        minutes = Mathf.FloorToInt(timeRemaining / 60);
        seconds = Mathf.FloorToInt(timeRemaining % 60);
        // timerText.text = timeRemaining.ToString();

        timerText.text = String.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public string GetTime() {
        return timerText.text;
    }
}
