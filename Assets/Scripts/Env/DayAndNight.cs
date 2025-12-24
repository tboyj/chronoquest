using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform sunBody;
    public AnimationCurve lightCurve;
    [Range(-90f, 270f)]
    public float timeInDay = 0f;
    private float rotationSpeed = .005f;
    public TextMeshProUGUI timeText;
    [SerializeField]
    public GameObject timeCube;
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        // GameObject dateObj = GameObject.Find("Time"); // Less ideal but works
        // timeText = dateObj.GetComponent<TextMeshProUGUI>();
        timeText.text = "06:00";
        gameObject.transform.rotation = Quaternion.Euler(0, -90f, -90f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string amOrPm = "AM";
        float multiplier = lightCurve.Evaluate(timeInDay);
        timeInDay += rotationSpeed * multiplier;
        int hours = Mathf.FloorToInt((timeInDay + 90f) / 15f);
        int minutes = Mathf.FloorToInt((((timeInDay + 90f) / 15f) - hours) * 60f);

        

        hours = (hours + 24) % 24; // Ensure hours is always 0-23
        int displayHour = hours;
        if (displayHour >= 12)
        {
            amOrPm = "PM";
            if (displayHour > 12)
                displayHour -= 12;
        }
        if (displayHour == 0)
            displayHour = 12;

        string hNotMil = displayHour.ToString("00");
        timeText.text = hNotMil + ":" + minutes.ToString("00") + " " + amOrPm;
        if (timeInDay >= 270f)
        {
            timeCube.GetComponent<TimeCube>().addDayToCalendar();
            timeInDay = -90f;
        }

        sunBody.rotation = Quaternion.Euler(timeInDay, -90f, -90f);

    }
    public void SetTimeCube()
    {

    }
    
    public void SetTimeOfDay(float time)
    {
        timeInDay = time;
    }
}
