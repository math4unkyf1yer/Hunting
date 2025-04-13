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

    public int deerCount;

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
            if(deerCount>= 4)
            {
                //you win
            }
            else
            {
                //you lose 
            }
            SceneManager.LoadScene(0);
        }
    }
}
