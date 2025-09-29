using TMPro;
using UnityEngine;

public class QuestManagerGUI : MonoBehaviour
{
    public TextMeshProUGUI currentQuestName;
    public TextMeshProUGUI currentQuestCondition;
    QuestManager qm;
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
                qm.questsAssigned[0].todo.RemoveAt(0);
                currentQuestCondition.text = qm.questsAssigned[0].todo[0];
            }

        }
    }
}