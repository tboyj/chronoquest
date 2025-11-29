using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionReciever : MonoBehaviour
{
    [SerializeField]
    private List<RaycastFounder> reflections;
    public GameObject barrier;
    public bool ConditionsMet;
    public void CheckAllReflections()
    {
        bool allAreReflecting = true;
        Debug.Log("Recieved the last one!");
        for (int i = 0; i < reflections.Count; i++)
        {
            if (reflections[i] != null)
            {
                if (!reflections[i].activeReflection)
                {
                    Debug.Log("At least one mirror is still not reflecting within the series.");
                    allAreReflecting = false;
                    break;
                }
            }
        }
        if (allAreReflecting) {
            Debug.Log("Everything is correct.");
            ConditionsMet = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // depends on save data!!!
        barrier.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ConditionsMet)
        {
            // Advance quest
            barrier.SetActive(false);
        }
    }
}
