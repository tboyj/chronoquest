using System.Collections.Generic;
using ChronoQuest.Quests;
using UnityEngine;

[System.Serializable]
public class QuestInstance : MonoBehaviour
{
    public Quest data;
    public IQuestAction condition;
    public bool IsCompleted;
    public List<string> todo;

    public QuestInstance(Quest q, bool i, List<string> t)
    {
        data = q;
        IsCompleted = i;
        todo = t;
    }
    public void Trigger()
    {
        condition.QuestEventTriggered();
    }
    public virtual bool CheckConditions()
    {
        return IsCompleted;
    }

}