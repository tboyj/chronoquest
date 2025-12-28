using System;
using UnityEngine;

public class MidQuestDialog : MonoBehaviour
{
    [SerializeField]
    private QuestInstance parentQuest;
    [SerializeField]
    protected GameObject obj;
    void Start()
    {
        parentQuest = gameObject.transform.parent.GetComponent<QuestInstance>();
        if (parentQuest != null)
        {
            obj.SetActive(true);
            if (parentQuest.data.id <= CurrentQIDMonitor.Instance.GetCurrentQuestId())
            {
                ActionMidQuest();
            } else
            {
                obj.SetActive(false);
            }
        }
    }
    public virtual void ActionMidQuest()
    {
        Debug.Log("Default");
        // Debug.Log("Default");
        // Debug.Log("Default");
        // Debug.Log("Default");
    }
}