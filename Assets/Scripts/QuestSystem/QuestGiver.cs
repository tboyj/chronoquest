using System;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public CollectItemQuest questToGive;
    [SerializeField]
    private ItemStorable itemToCollect;
    public int id;
    void Start()
    {
        
    }
    public CollectItemQuest CollectItem()
    {
        return questToGive.CollectItemInitialize(1, 5, 0, itemToCollect);
    }
}
