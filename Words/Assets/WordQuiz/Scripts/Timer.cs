using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public static int t = 480;
    public static float timeRemaining = t;
    public static bool timerIsRunning = false;
    public Text timeText;
    [SerializeField] public GameObject gameLose;
    public  AudioSource audioSource1;
    [SerializeField]
    private Button resetBtn;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                //Debug.Log("Time has run out!");
                audioSource1.Stop();
                resetBtn.gameObject.SetActive(true);
                gameLose.SetActive(true);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    public static void timeStart() {
        timeRemaining = t;
        timerIsRunning = true;
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}