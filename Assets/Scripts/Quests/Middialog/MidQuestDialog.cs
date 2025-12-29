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
        obj.SetActive(false);
        // parentQuest = gameObject.transform.parent.GetComponent<QuestInstance>();
        if (parentQuest != null)
        {
            
            if (parentQuest.data.id == CurrentQIDMonitor.Instance.GetCurrentQuestId())
            {
                obj.SetActive(true);
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