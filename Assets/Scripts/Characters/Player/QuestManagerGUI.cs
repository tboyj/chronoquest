using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class QuestManagerGUI : MonoBehaviour
{
    public TextMeshProUGUI currentQuestName;
    public TextMeshProUGUI currentQuestCondition;
    public QuestManager qm;
    
    public void Start()
    {
        qm = gameObject.GetComponent<QuestManager>();
    }
    public void RefreshQuestGUI()
    {
        if (qm.questsAssigned.Count > 0)
        {
            currentQuestName.text = qm.questsAssigned[0].data.questName;
            currentQuestCondition.text = qm.questsAssigned[0].todo[0];

        }

    }
    public void GotoNextTodo()
    {
        if (qm.questsAssigned.Count > 0)
        {
            if (qm.questsAssigned[0].todo.Count > 1)
            {
                bool isWorking = true;
                foreach (QuestInstance relatedQuest in qm.GetCurrentQuest().relatedQuests)
                {
                    if (!relatedQuest.IsCompleted)
                    {
                        Debug.Log("Not completed.");
                        isWorking = false;
                        break;
                    }
                }
                if (isWorking)
                {
                    qm.questsAssigned[0].todo.RemoveAt(0);
                    currentQuestCondition.text = qm.questsAssigned[0].todo[0];
                }
            }

        }
    }
}