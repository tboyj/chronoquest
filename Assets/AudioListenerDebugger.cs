using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public class AudioListenerDebugger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool logOnStart = true;
    [SerializeField] private bool logOnSceneLoad = true;
    [SerializeField] private KeyCode manualLogKey = KeyCode.L;

    void Start()
    {
        if (logOnStart)
        {
            LogAllAudioListeners();
        }

        if (logOnSceneLoad)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(manualLogKey))
        {
            LogAllAudioListeners();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogAllAudioListeners();
    }

    [ContextMenu("Log All Audio Listeners")]
    public void LogAllAudioListeners()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>(true); // Include inactive
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"=== AUDIO LISTENERS FOUND: {listeners.Length} ===");
        
        if (listeners.Length == 0)
        {
            sb.AppendLine("⚠️ NO AUDIO LISTENERS FOUND!");
        }
        else if (listeners.Length > 1)
        {
            sb.AppendLine("⚠️ WARNING: Multiple AudioListeners detected! Only one should be active.");
        }
        
        for (int i = 0; i < listeners.Length; i++)
        {
            AudioListener listener = listeners[i];
            sb.AppendLine($"\n[{i}] AudioListener:");
            sb.AppendLine($"  Path: {GetGameObjectPath(listener.gameObject)}");
            sb.AppendLine($"  Scene: {listener.gameObject.scene.name}");
            sb.AppendLine($"  Active: {listener.gameObject.activeInHierarchy}");
            sb.AppendLine($"  Enabled: {listener.enabled}");
        }
        
        sb.AppendLine("\n=== LOADED SCENES ===");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            sb.AppendLine($"  [{i}] {scene.name} (Active: {scene == SceneManager.GetActiveScene()})");
        }
        
        Debug.Log(sb.ToString());
    }

    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform.parent;
        
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        
        return path;
    }
}