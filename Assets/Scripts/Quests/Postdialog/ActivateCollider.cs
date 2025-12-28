using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollider : AfterQuestDialog
{
    // Start is called before the first frame update
    public Collider collider;
    [SerializeField]
    protected QuestInstance quest;

    void Start()
    {
        quest = GetComponent<QuestInstance>();
        if (quest.data.id <= CurrentQIDMonitor.Instance.GetCurrentQuestId())
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void SetChange()
    {
        collider.enabled = true;
    }
}