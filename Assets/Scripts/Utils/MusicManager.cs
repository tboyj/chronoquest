using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PauseScript pause;
    public AudioSource audio;
    void Start()
    {
        pause = gameObject.GetComponent<PauseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause != null) {
            if (pause.isPaused && audio.isPlaying)
            {
                audio.Pause();
            } else if (!pause.isPaused && !audio.isPlaying)
            {
                audio.UnPause();
            }
        }
    }
}
