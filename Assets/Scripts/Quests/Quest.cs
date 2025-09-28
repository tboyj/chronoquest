using System;
using System.Collections.Generic;
using ChronoQuest.Quests;
using UnityEngine;

public class Quest : ScriptableObject
{
    public string questName;
    public int id;
    public List<string> todo;
    public IQuestAction condition;
    public bool IsCompleted;
    public void Trigger()
    {
        condition.QuestEventTriggered();
    }
    public virtual bool CheckConditions()
    {
        return IsCompleted;
    }


}

