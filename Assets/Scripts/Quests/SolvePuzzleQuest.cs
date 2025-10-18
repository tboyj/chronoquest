using System.Collections;
using System.Collections.Generic;
using ChronoQuest.Quests;
using UnityEngine;

public class SolvePuzzleQuest : QuestInstance, IQuestAction
{
    // Start is called before the first frame update
    public SolvePuzzleQuest(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s) : base(q, i, t, d, s)
    {
    }
}
