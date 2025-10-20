using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignNewQuest : ExtraBase
{
    QuestHandler qh;
    public override void Change()
    { // Check check :)s\
        Debug.Log("Changing right now... Dattebayo!");
        if (qh.GetQuestList().Count > 0 && qm.GetCurrentQuest() != null)
        {
            qm.GetCurrentQuest().IsCompleted = true;
            qm.SetQuestCompleted(qm.GetCurrentQuest());
            qm.AddQuestToList(qh.GetMostRecentQuest());
            Debug.Log("skull emoji");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        qh = gameObject.GetComponent<QuestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Keybinds.actionKeybind) && !player.gameObject.GetComponent<PauseScript>().isPaused &&
        !player.gameObject.GetComponent<PauseScript>().isInventory && inRange)
        {
            Change(); // attempts to change.
        }
    }
}
