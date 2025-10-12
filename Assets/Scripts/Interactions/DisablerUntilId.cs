using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DisablerUntilId : MonoBehaviour
{
    // Start is called before the first frame update
    public QuestManager playerManager;
    public int currentQuestId;
    public SphereCollider blocker;
    void Awake()
    {
        blocker = transform.parent.gameObject.GetComponent<SphereCollider>();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();
    }
    void Start()
    {
        
        IdUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        IdUpdate();
    }

    private void IdUpdate()
    {
        if (playerManager?.GetCurrentQuest())
        {
            currentQuestId = playerManager.GetCurrentQuest().data.id;
            if (!CompareId())
            {
                blocker.enabled = false;
            } else
            {
                blocker.enabled = true;
            }
        } else
        {
            blocker.enabled = false;
        }
    }
    private bool CompareId()
    {
        if (playerManager.GetCurrentQuest().data.id == currentQuestId)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
