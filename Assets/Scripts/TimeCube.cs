using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    public string sceneName;
    public string reformattedDate;
    public int daysLived;
    void Start()
    {
        sceneName = gameObject.scene.name;
        Debug.Log("Scene Name: " + sceneName);
        switch (sceneName)
        {
            case "SampleScene":
                currDay = 1;
                currMonth = 1;
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
        if (currYear < 0)
        {
            isAD = false;
        }
        else
        {
            isAD = true;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}