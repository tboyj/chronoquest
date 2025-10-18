using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectOrderOpenScript : MonoBehaviour
{
    // Start is called before the first frame update

    public int idSave;
    public List<GameObject> guessOrder = new List<GameObject>();
    public List<GameObject> answerOrder = new List<GameObject>();
    public QuestToggleItem quest;

    public void CheckOrder(GameObject check)
    {
        int i = 0;
        bool ableToOpen = true;
        guessOrder.Add(check);
        if (guessOrder.Count == answerOrder.Count)
        {
            
            while (i < guessOrder.Count)
            {
                if (guessOrder[i] != answerOrder[i])
                {
                    ableToOpen = false;
                }
                i++;
            }
            if (ableToOpen)
                OpenDoor();
            else
                ResetGuess();
            
        }
    }

    private void ResetGuess()
    {
        guessOrder.Clear();
        foreach (GameObject button in answerOrder)
        {
            button.GetComponent<PuzzleLeverActivator>().conditionsMet = false;
            button.GetComponent<PuzzleLeverActivator>().toggled = false;
            button.GetComponent<PuzzleLeverActivator>().WrongCoroutineExecutor();
        }

    }


    private void OpenDoor()
    {
        
        foreach (GameObject button in answerOrder)
        {
            button.GetComponent<PuzzleLeverActivator>().sprite.color = Color.white;
            button.GetComponent<PuzzleLeverActivator>().conditionsMet = true;
            if (button.GetComponent<PuzzleLeverActivator>().playerQuest != null)
                idSave = button.GetComponent<PuzzleLeverActivator>().playerQuest.data.id;
        }
        gameObject.transform.position = new Vector3(transform.position.x, (transform.position.y - 3), transform.position.z);
        if (quest != null && idSave == quest.data.id)
        {
            quest.QuestEventTriggered();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
