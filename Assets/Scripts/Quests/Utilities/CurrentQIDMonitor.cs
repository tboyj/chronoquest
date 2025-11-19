using UnityEngine;

public class CurrentQIDMonitor : MonoBehaviour
{
    public static CurrentQIDMonitor Instance { get; private set; }

    [SerializeField]
    private int currentQuestId;

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // If you want this to persist across scene loads:
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentId(int id)
    {
        currentQuestId = id;
    }

    public int GetCurrentQuestId()
    {
        return currentQuestId;
    }
}
