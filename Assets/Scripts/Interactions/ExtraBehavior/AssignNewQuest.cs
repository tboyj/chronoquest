using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignNewQuest : ExtraBase
{
    QuestHandler qh;
    public override void Change()
    { // Check check :)s\
        Debug.Log("Changing right now... Dattebayo!");
        if (qh.GetQuestList().Count > 0) //  && qm.GetCurrentQuest() != null && qm.GetCurrentQuest().IsCompleted
        {
            qm.AddQuestToList(qh.GetMostRecentQuest());
            CurrentQIDMonitor.Instance.SetCurrentId(qm.GetCurrentQuest().data.id);

            Debug.Log($"[QuestManager] Advanced to quest {qm.GetCurrentQuest().data.id}");
        }
        else
        {
            Debug.Log("You missed a check");
        }
        qm.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        qh = gameObject.GetComponent<QuestHandler>();
        Debug.Log(qh);
    }

    // Update is called once per frame
}
