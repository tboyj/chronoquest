using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform sunBody;
    public AnimationCurve lightCurve;
    public Light lightForSky;
    [Range(-360f, 0.005f)]
    public float timeInDay;
    private float rotationSpeed = .005f;
    public TextMeshProUGUI timeText;
    void Start()
    {
        timeText.text = "00:00";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isPM = false;
        string amOrPm = "AM";
        float multiplier = lightCurve.Evaluate(timeInDay);
        timeInDay += rotationSpeed * multiplier;
        int hours = Mathf.FloorToInt((timeInDay + 360f) / 15f) + 6;
        int minutes = Mathf.FloorToInt((((timeInDay + 360f) / 15f) + 6 - hours) * 60f);
        if (hours % 13 == 0 && hours % 24 != 0)
        {
            isPM = true;
        }
        else if (hours % 24 == 0)
        {
            isPM = false;
        }
        hours %= 24;
        string hNotMil = hours.ToString("00");
        if (hours == 0)
        {
            hNotMil = "01";
        }
        if (isPM)
        {
            hNotMil = (hours - 12).ToString("00");
            amOrPm = "PM";
        }
        else
            amOrPm = "AM";
        timeText.text =  hNotMil+ ":" + minutes.ToString("00")+" "+amOrPm;
        if (timeInDay >= 0.005f)
        {
            DaysHandler.daysLived++;
            timeInDay = -360f;
        }

        sunBody.rotation = Quaternion.Euler(timeInDay, -90f, -90f);

    }
}
