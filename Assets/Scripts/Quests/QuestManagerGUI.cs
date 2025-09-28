using TMPro;
using UnityEngine;

public class QuestManagerGUI : MonoBehaviour
{
    public TextMeshProUGUI currentQuestName;
    public TextMeshProUGUI currentQuestCondition;

    public void RefreshQuestGUI()
    {
        if (gameObject.GetComponent<QuestManager>().questsAssigned.Count > 0)
        {
            currentQuestName.text = gameObject.GetComponent<QuestManager>().questsAssigned[0].questName;
            currentQuestCondition.text = gameObject.GetComponent<QuestManager>().questsAssigned[0].todo[0];

        }
    }
    public void GotoNextTodo()
    {
        if (gameObject.GetComponent<QuestManager>().questsAssigned.Count > 0)
        {
            if (gameObject.GetComponent<QuestManager>().questsAssigned[0].todo.Count > 1)
            {
                gameObject.GetComponent<QuestManager>().questsAssigned[0].todo.RemoveAt(0);
                currentQuestCondition.text = gameObject.GetComponent<QuestManager>().questsAssigned[0].todo[0];
            }

        }
    }
}