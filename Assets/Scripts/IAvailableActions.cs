using UnityEngine;

namespace ChronoQuest.UIForInteractions
{
    public interface IAvailableActions
    {
        // Start is called before the first frame update
        public void ChangeTheUI(string str);
        public void ChangeTheUI(Item item);
    }
}