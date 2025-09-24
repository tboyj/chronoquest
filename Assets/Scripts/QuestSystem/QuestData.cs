using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Collect Item Quest 2")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string description;
    public bool isRepeatable;
    public enum QuestType { Main, Side, Mini };
    public QuestType questType;
    
}
