// using System;
// using System.IO;
// using UnityEngine;

// [Serializable]
// public class SceneSongData
// {
//     public string clipName;
//     public string sceneName;
// }

// public class SceneSongHandler : MonoBehaviour
// {
//     private AudioSource audioSource;
//     private string saveFilePath;

//     void Awake()
//     {
//         audioSource = GetComponent<AudioSource>();
//         saveFilePath = Path.Combine(Application.persistentDataPath, "scenesong.json");
//     }

//     // Save current audio clip info
//     public void SaveCurrentSong()
//     {
//         if (audioSource == null || audioSource.clip == null)
//         {
//             Debug.LogWarning("No AudioSource or clip to save");
//             return;
//         }

//         SceneSongData data = new SceneSongData
//         {
//             clipName = audioSource.clip.name,
//             sceneName = gameObject.scene.name
//         };

//         string json = JsonUtility.ToJson(data, true);
//         File.WriteAllText(saveFilePath, json);
        
//         Debug.Log($"Saved song: {data.clipName} for scene: {data.sceneName} to {saveFilePath}");
//     }

//     // Load audio clip info
//     public SceneSongData LoadSongData()
//     {
//         if (!File.Exists(saveFilePath))
//         {
//             Debug.LogWarning("No song save file found");
//             return null;
//         }

//         string json = File.ReadAllText(saveFilePath);
//         SceneSongData data = JsonUtility.FromJson<SceneSongData>(json);
        
//         Debug.Log($"Loaded song data: {data.clipName} from scene: {data.sceneName}");
//         return data;
//     }

//     // Get current clip name without saving
//     public string GetCurrentClipName()
//     {
//         return audioSource?.clip?.name ?? "";
//     }

//     // Delete the save file
// }