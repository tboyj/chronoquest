using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCube : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("The starting day.")]
    public int startDay;

    [Tooltip("The current day.")]
    public int currDay;

    [Tooltip("The starting month.")]
    public int startMonth;

    [Tooltip("The current month.")]
    public int currMonth;

    [Tooltip("The starting year.")]
    public int startYear;

    [Tooltip("The current year.")]
    public int currYear;

    public bool isAD;
    [SerializeField]
    public string sceneName;
    public string reformattedDate;
    [SerializeField]
    public TextMeshProUGUI dateText;
    public PixelateEffect bitsChanger;

    void Start()
    {

        sceneName = gameObject.scene.name;
        Debug.Log("Scene Name: " + sceneName);
        GameObject canvas = GameObject.Find("Canvas");
        GameObject dateObj = canvas.transform.Find("HideForDialogContainer/GameUIHolder/Bg_Date/Date").gameObject;
        dateText = dateObj.GetComponent<TextMeshProUGUI>();
        bitsChanger = GameObject.Find("RealPlayer").transform.Find("MainCamera").gameObject.GetComponent<PixelateEffect>();


        switch (sceneName)
        {
            case "SampleScene":
                currDay = 1;
                currMonth = 1;
                currYear = 2024;
                bitsChanger.SetPixelSize(1);
                break;
            case "Tutorial":
                currDay = 6;
                currMonth = 7;
                currYear = 2024;
                break;
            case "AncientEgypt":
                currDay = 15;
                currMonth = 6;
                currYear = -200;
                break;
            case "Level3":
                currDay = 25;
                currMonth = 12;
                currYear = 3000;
                break;
            default:
                Debug.LogError("Unknown scene: " + sceneName);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        if (currYear < 0)
        {
            isAD = false;
        }
        else
        {
            isAD = true;
        }
        reformattedDate = currMonth.ToString("00") + "/" + currDay.ToString("00") + "/" + Mathf.Abs(currYear).ToString() + (isAD ? " AD" : " BC");
        // For testing purposes, you can uncomment the line below to see the date in the console
        dateText.text = reformattedDate;
    }
    public void addDayToCalendar()
    {
        // Implement your logic here, for example:
        currDay++;
        if (currDay > 30) // Simplified month length
        {
            currDay = 1;
            currMonth++;
            if (currMonth > 12)
            {
                currMonth = 1;
                currYear++;
                if (currYear == 0) // There is no year 0
                {
                    currYear = 1;
                }
            }
        }

        Debug.Log("A new day has been added to the calendar.");
    }
}