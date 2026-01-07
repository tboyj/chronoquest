using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestDialog
{
    public string dialogueText;
    public string characterName;
    public List<MidQuestDialog> midQuestActions;
    public void CycleMidQuestActions()
    {
        foreach (MidQuestDialog a in midQuestActions)
        {
            if (a != null)
                a.ActionMidQuest();
        }
    }
}