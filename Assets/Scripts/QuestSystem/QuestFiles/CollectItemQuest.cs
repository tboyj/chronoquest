using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectItemQuest", menuName = "Quest System/Collect Item Quest")]
public class CollectItemQuest : Quest
{
    public int requiredItemId;
    public int requiredAmount;
    private int itemsCollected;
    public ItemStorable itemNeeded;
    public override void Initialize()
    {
        itemsCollected = 0;
        isCompleted = false;
    }

    public void ReportItemCollected(int itemId, int amount)
    {
        if (itemId == requiredItemId)
        {
            itemsCollected += amount;
            CheckProgress();
        }
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