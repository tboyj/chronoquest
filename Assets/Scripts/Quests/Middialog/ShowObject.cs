using System;
using UnityEngine;

public class ShowObject : MidQuestDialog
{
    public GameObject obj;
    public void Start()
    {
        obj.SetActive(false);
    }
    public override void ActionMidQuest()
    {
        Debug.Log($"Attempting {obj.name} to true");
        if (obj == null)
        {
            Debug.LogWarning("Object not assigned. Cannot function." );
            return;
        }
        obj.SetActive(true);
    }
}
