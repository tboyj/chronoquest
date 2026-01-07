using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    private PauseScript pause;
    private AudioSource audio;
    void Start()
    {
        
        pause = GetComponent<PauseScript>();
        audio = GameObject.Find("SceneSong").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.isPaused && audio.isPlaying)
        {
            audio.Pause();
        } else if (!pause.isPaused && !audio.isPlaying)
        {
            audio.UnPause();
        }
    }
}
