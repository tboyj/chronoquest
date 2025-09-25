using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectItemQuest", menuName = "Quest System/Collect Item Quest")]
public class CollectItemQuest : Quest
{
    public int requiredItemId;
    public int requiredAmount;
    public int itemsCollected;
    public ItemStorable itemNeeded;
    public override void Initialize()
    {
        itemsCollected = 0;
        isCompleted = false;
    }
    public virtual CollectItemQuest CollectItemInitialize(int requiredItemId, int requiredAmount, int itemsCollected, ItemStorable itemNeeded)
    {
        this.requiredItemId = requiredItemId;
        this.requiredAmount = requiredAmount;
        this.itemsCollected = itemsCollected;
        this.itemNeeded = itemNeeded;
        return this;
    }

    public void ReportItemCollected(int amount)
    {
            itemsCollected+=amount;
            Debug.Log("Added item to collection");
            CheckProgress();
    }
    public void SetQuestName(string name)
    {
        questName = name;
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NPC npcHolder = this.GetComponent<NPC>();
            // Make it so that it checks here. add in function later.
        }
    }

    public override void CheckProgress()
    {
        if (itemsCollected >= requiredAmount)
        {
            isCompleted = true;
            Debug.Log($"Quest '{questName}' completed!");
        }
    }
}