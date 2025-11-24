using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignNewQuest : ExtraBase
{
    QuestHandler qh;
    public override void Change()
    { // Check check :)s\
        Debug.Log("Changing right now... Dattebayo!");
        if (qh.GetQuestList().Count > 0 && qm.GetCurrentQuest() != null && qm.GetCurrentQuest().IsCompleted) // problemm 
        {
            qm.SetQuestCompleted(qm.GetCurrentQuest());
            var nextQuest = qh.GetMostRecentQuest();
            qm.AddQuestToList(nextQuest);
            CurrentQIDMonitor.Instance.SetCurrentId(nextQuest.data.id);

            Debug.Log($"[QuestManager] Advanced to quest {nextQuest.data.id}");
        }
        qm.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        qh = gameObject.GetComponent<QuestHandler>();
        Debug.Log(qh);
    }

    // Update is called once per frame
}
