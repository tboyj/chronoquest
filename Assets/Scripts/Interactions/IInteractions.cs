namespace ChronoQuest.Interactions.World
{
    public interface Interaction
    {
        void InteractionFunction();
        bool inDialog { get; set; }
    }
}