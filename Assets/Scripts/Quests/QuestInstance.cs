using System;
using System.Collections.Generic;
using ChronoQuest.Quests;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // private SphereCollider parentCollider;
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

    public void ReinitializeConditions()
    {
        if (this is IQuestAction action)
        {
            Debug.Log("Reinitializing conditions for quest: " + data.id);
            Debug.Log(action);
            conditions = action;
            // try to see if you can put a check on isComplete here for your quests, as you need that as well to define the point in the quest.
        }
        
    }
    public virtual void Start()
    {
        // parentCollider = gameObject.transform.parent.GetComponent<SphereCollider>();
        // questManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();
        positionOfQuestGiver = gameObject.transform.parent.position;
        
        if (SceneManager.GetSceneByName("UtilityScene").isLoaded)
        {
            if (CurrentQIDMonitor.Instance.GetCurrentQuestId() > data.id)
            {
                gameObject.transform.parent.position = new Vector3(0, -100, 0);
            }
            if (CurrentQIDMonitor.Instance.GetCurrentQuestId() <= data.id)
            {
                gameObject.transform.parent.position = positionOfQuestGiver;
            }
        }
        
        // ONLY trigger if this is the current quest
        // if (conditions != null && CurrentQIDMonitor.Instance != null)
        // {
        //     if (CurrentQIDMonitor.Instance.GetCurrentQuestId() == data.id)
        //     {
        //         conditions.QuestEventTriggered();
        //     }
        // }
    }
    public void Update()
    {
        if (questManager != null && CurrentQIDMonitor.Instance != null)
        {
            int currentQuestId = CurrentQIDMonitor.Instance.GetCurrentQuestId();
            HandlingQuestId(currentQuestId);
        }
    }
    private void HandlingQuestId(int currentQuestId)
    {
        if (currentQuestId > 0) // Check if there is a valid current quest ID
        {
            // Show only the current quest's NPC, hide all others
            if (currentQuestId == data.id)
            {
                // This is the active quest - show the NPC
                gameObject.transform.parent.position = positionOfQuestGiver;
            }
            else
            {
                // This is not the active quest - hide the NPC
                gameObject.transform.parent.position = new Vector3(0, -100, 0);
            }
        }
        else
        { 
            // No current quest - disable collider and hide NPC
            gameObject.transform.parent.position = new Vector3(0, -100, 0); // moves the quest giver npc back out of sight
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