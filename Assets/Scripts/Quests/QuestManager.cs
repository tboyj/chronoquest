using System;
using System.Collections.Generic;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public List<Quest> questsAssigned = new List<Quest>();
    public List<Quest> questsCompleted = new List<Quest>();
    public void AddQuestToList(Quest quest)
    {
        questsAssigned.Add(quest);
        gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }
    public void SetQuestCompleted(Quest quest)
    {
        questsCompleted.Add(quest);
        questsAssigned.Remove(quest);
        gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }

    public bool PrecheckQuest(Quest questAssigned)
    {
        return questAssigned.CheckConditions();
    }
}