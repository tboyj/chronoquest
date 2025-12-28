using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerEndgameParse : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool saveTimerOnDisplay = true;

    // Start is called before the first frame update
    void Start()
    {
        DisplayFinalTime();
    }

    void DisplayFinalTime()
    {
        // Check if TimerJSON instance exists
        if (TimerJSON.Instance != null)
        {
            // Stop the timer
            TimerJSON.Instance.StopTimer();
            
            // Save the timer if desired
            if (saveTimerOnDisplay)
            {
                TimerJSON.Instance.SaveTimer();
            }
            
            // Get the formatted time and display it
            string finalTime = TimerJSON.Instance.GetFormattedTime();
            
            if (timerText != null)
            {
                timerText.text = "Final Time: " + finalTime;
                Debug.Log($"Final time displayed: {finalTime}");
            }
            else
            {
                Debug.LogError("TextMeshProUGUI reference is not set in TimerEndgameParse!");
            }
        }
        else
        {
            Debug.LogError("TimerJSON instance not found! Make sure TimerJSON exists in the scene.");
            
            if (timerText != null)
            {
                timerText.text = "N/A";
            }
        }
    }

    // Optional: Method to get raw time value if needed for other purposes
    public float GetFinalTime()
    {
        if (TimerJSON.Instance != null)
        {
            return TimerJSON.Instance.GetElapsedTime();
        }
        return 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}