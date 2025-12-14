using System;
using System.Collections.Generic;
using ChronoQuest.Quests;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestInstance : MonoBehaviour
{
    
    public Quest data;
    public IQuestAction conditions;
    public bool IsCompleted;
    public List<TodoObject> todo;
    [SerializeField]
    public List<QuestDialog> dialogsForQuest;
    public QuestManager questManager;
    public List<QuestInstance> relatedQuests;
    public Vector3 positionOfQuestGiver;
    [SerializeReference]
    public List<AfterQuestDialog> postQuestList;
    // public QuestInstance(Quest q, bool i, List<TodoObject> t, List<QuestDialog> d, List<QuestInstance> s)
    // {
    //     data = q;
    //     IsCompleted = i;
    //     todo = t;
    //     dialogsForQuest = d;
    //     relatedQuests = s;
    // }
    public void Awake()
    {
        
    }
    public virtual void Start()
    {
        questManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();
        positionOfQuestGiver = gameObject.transform.parent.position;
    }
    public void Update()
    {
        if (questManager != null)
        {
            if (questManager.GetCurrentQuest() != null)
            {
                // Debug.Log("Current Quest ID: " + questManager.GetCurrentQuest().data.id + " This Quest ID: " + data.id);
                if (questManager.GetCurrentQuest().data.id < data.id) // checks if the current quest is before this quest in the quest line
                {
                    gameObject.transform.parent.GetComponent<SphereCollider>().enabled = false;
                    gameObject.transform.parent.position = new Vector3(0, -100, 0); // moves the quest giver npc out of sight
                }
                else
                {
                    gameObject.transform.parent.GetComponent<SphereCollider>().enabled = true;
                    gameObject.transform.parent.position = positionOfQuestGiver; // moves the quest giver npc back to original position
                    if (questManager.GetCurrentQuest().data.id == data.id) {
                    
                        
                    }
                }
            }
            else
            { // else SCREW THAT !!! we want the collider to be inactive if there is no current quest
              // Debug.Log("I couldn't work because i'm just a silly boy: "+questManager.gameObject.name);
                gameObject.transform.parent.GetComponent<SphereCollider>().enabled = false;
                gameObject.transform.parent.position = new Vector3(0, -100, 0); // moves the quest giver npc back out of sight
            }

        }
    } 
    public void Trigger()
    {
        Debug.Log($"Condition {conditions}");
        conditions.QuestEventTriggered();
    }
    public virtual bool CheckConditions()
    {
        
        return IsCompleted;
    }

    public void DialogAdvance()
    {
        if (dialogsForQuest.Count > 0)
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

    public virtual void QuestEventTriggered()
    {
        Debug.Log("Default");
    }
}