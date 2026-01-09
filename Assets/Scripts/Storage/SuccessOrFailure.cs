using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SuccessOrFailure : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite imageSuccess;
    public Sprite imageFailure;
    public Image background;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    [Header("Success/Failure Settings")]
    public Color successColor = Color.green;
    public Color failureColor = Color.red;
    public float timeLimit = 600f; // 10 minutes in seconds

    void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        
        CheckMissionResult();
    }

    void CheckMissionResult()
    {
        if (TimerJSON.Instance != null)
        {
            // Stop the timer
            TimerJSON.Instance.StopTimer();
            
            // Get elapsed time
            float finalTime = TimerJSON.Instance.GetElapsedTime();
            string formattedTime = TimerJSON.Instance.GetFormattedTime();
            
            // Check if mission was successful (under 10 minutes)
            if (finalTime < timeLimit)
            {
                // Mission Success
                background.sprite = imageSuccess;
                titleText.text = "Mission Successful";
                descriptionText.text = $"You were able to save the universe. Now you can go back to your family...and maybe quit your job as Bossman's henchman.";
                Debug.Log($"Mission Success! Time: {formattedTime}");
            }
            else
            {
                // Mission Failed
                background.sprite = imageFailure;
                titleText.text = "Mission Failed";
                descriptionText.text = $"You end up stuck in space floating around with Grainly floating about. You curse Bossman forever for not going with you...";
                Debug.Log($"Mission Failed! Time: {formattedTime}");
            }
            
            // Save the timer
            TimerJSON.Instance.SaveTimer();
        }
        else
        {
            Debug.LogError("TimerJSON instance not found!");
            titleText.text = "Fractured! (Error)";
            descriptionText.text = "Couldn't find your time...seems like you fractured it too much.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}