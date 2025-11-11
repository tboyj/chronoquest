using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSceneQuest : MonoBehaviour
{
    // Start is called before the first frame update
    public QuestManager questManager;
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void RuntimeQuest()
    {
        if (transform.childCount == 0)
        {
            Debug.LogError("No starting quest found. You may be corrupting save data!");
            return;
        }

        if (questManager == null)
        {
            Debug.LogError("QuestManager unassigned (null ex)");
            return;
        }

        Debug.Log("running test");
        QuestInstance firstQuest = transform.GetChild(0).GetComponent<QuestInstance>();
        // Marks current quest as completed if necessary
        if (questManager.GetCurrentQuest())
            questManager.GetCurrentQuest().IsCompleted = true;
        // Check ID and assign if appropriate
        if (firstQuest != null)
        {
            questManager.AddQuestToList(firstQuest);
            questManager.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        }
 
    }
}
