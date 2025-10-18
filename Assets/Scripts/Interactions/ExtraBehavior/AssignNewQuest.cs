using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignNewQuest : ExtraBase
{
    public QuestInstance questToAdd;
    public override void Change()
    { // Check check :)s
        player.GetComponent<QuestManager>().AddQuestToList(questToAdd);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<QuestManager>().GetCurrentQuest() == null)
        {
            Change();
        }
    }
}
