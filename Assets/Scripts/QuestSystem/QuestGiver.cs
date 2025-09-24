using System;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public CollectItemQuest questToGive;
    
    void Start()
    {
        questToGive.Initialize();
    }
    
}
