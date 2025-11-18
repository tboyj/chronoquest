using System.Collections.Generic;
using UnityEngine;

public class TodoObject : MonoBehaviour
{
    public List<QuestDialog> dialogsForQuestSections;
    public Transform accessPoint;
    public string todoText;

    public void Start()
    {
        accessPoint = gameObject.transform;
    }
    public void Update()
    {
        accessPoint = gameObject.transform;
    }
    public Transform GetTodoAp() // gets access point (pointer for todo)
    {
        return accessPoint;
    }
    public void ShowDialog(bool v)
    {
        if (v)
        {
            Debug.Log(dialogsForQuestSections[0].characterName + ": " + 
            dialogsForQuestSections[0].dialogueText);
        }
    }
    public string GetCharName(int i)
    {
        return dialogsForQuestSections[i].characterName;
    }
    public string GetCharText(int i)
    {
        return dialogsForQuestSections[i].dialogueText;
    }
}