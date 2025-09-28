using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Silent Quest")]
public class SilentQuest : Quest
{
    [Tooltip("This script means nothing. Acts as a template.")]

    
    public void QuestEventTriggered()
    {
         // does nothing
    }

    public override bool CheckConditions()
    {
        return true; // checks for nothing
    }

}