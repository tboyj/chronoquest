using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeInDay < 0.005f && timeInDay >= -360f)
        {
            float multiplier = lightCurve.Evaluate(timeInDay);
            timeInDay += 1 * rotationSpeed * multiplier;

        }
        else
        {
            if (timeInDay >= 0.005f)
            {
                DaysHandler.daysLived++;
            }
            timeInDay = -360f;
        }
        sunBody.rotation = Quaternion.Euler(timeInDay, -90f, -90f);

    }
}
