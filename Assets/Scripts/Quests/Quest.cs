using System;
using System.Collections.Generic;
using ChronoQuest.Quests;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest Data")]
public class Quest : ScriptableObject
{
    public string questName;
    public int id;

}

