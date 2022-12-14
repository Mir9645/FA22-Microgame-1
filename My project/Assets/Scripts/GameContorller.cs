using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameContorller : MonoBehaviour
{
    public float GameTime;
    public bool TimerOn = false;
    public TMP_Text CountDownTimer;

    // Start is called before the first frame update
    void Start()
    {
        TimerOn = true;

    }

    void Update()
    {
        if (TimerOn)
        {
            if (GameTime > 0)
            {
                GameTime -= Time.deltaTime;
                DisplayTime(GameTime);
            }
            else
            {
                Debug.Log("Game End");
                GameTime = 0;
                TimerOn = false;
                SceneManager.LoadScene("GameClear");
            }
        }

       
    }
    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //    float milliSeconds = (timeToDisplay % 1) * 1000;
        //    CountDownTimer.text = string.Format("{1:00}:{2:000}", seconds, milliSeconds);
        //}
        CountDownTimer.text = "" + GameTime;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}
