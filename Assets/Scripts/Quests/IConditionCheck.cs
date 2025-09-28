namespace ChronoQuest.Quests
{
    public interface IQuestAction
    {
        void QuestEventTriggered();         // Called when the quest is triggered
        virtual bool IsCompleted()
        {
            return false;
        }    // Called to check if the quest is done
    }
}