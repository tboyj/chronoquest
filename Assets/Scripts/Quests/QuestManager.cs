using System;
using System.Collections.Generic;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public List<QuestInstance> questsAssigned = new List<QuestInstance>();
    public List<QuestInstance> questsCompleted = new List<QuestInstance>();
    private bool currentlyInDialog;

    public void AddQuestToList(QuestInstance quest)
    {
        questsAssigned.Add(quest);
        gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }
    public void SetQuestCompleted(QuestInstance quest)
    {
        questsCompleted.Add(quest);
        questsAssigned.Remove(quest);
        gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }

    public bool PrecheckQuest(QuestInstance questAssigned)
    {
        return questAssigned.CheckConditions();
    }

    public QuestInstance GetCurrentQuest()
    {
        if (questsAssigned.Count <= 0)
            return null;
        else
            return questsAssigned[0];
    }
    public bool CurrentlyInDialog()
    {
        return currentlyInDialog;
    }

    public void SetCurrentlyInDialog(bool v)
    {
        currentlyInDialog = v;
    }
}