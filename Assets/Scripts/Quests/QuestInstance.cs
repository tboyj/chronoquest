using System;
using System.Collections.Generic;
using ChronoQuest.Quests;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestInstance : MonoBehaviour
{
    public Quest data;
    public IQuestAction condition;
    public bool IsCompleted;
    public List<string> todo;
    [SerializeField]
    public List<QuestDialog> dialogsForQuest;
    public QuestManager questManager;

    public QuestInstance(Quest q, bool i, List<string> t, List<QuestDialog> d)
    {
        data = q;
        IsCompleted = i;
        todo = t;
        dialogsForQuest = d;
    }
    public void Trigger()
    {
        condition.QuestEventTriggered();
    }
    public virtual bool CheckConditions()
    {
        return IsCompleted;
    }

    public void DialogAdvance()
    {
        dialogsForQuest.RemoveAt(0);
    }

    public void ShowDialog(bool v)
    {
        if (v)
        {
            Debug.Log(dialogsForQuest[0].characterName + ": " + dialogsForQuest[0].dialogueText);
        }
    }
    public int GetQuestID()
    {
        return data.id;
    }
}