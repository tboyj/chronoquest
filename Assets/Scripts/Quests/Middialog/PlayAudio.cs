using System;
using UnityEngine;

public class PlayAudio : MidQuestDialog
{
    private AudioSource audio;

    public override void ActionMidQuest()
    {
        audio = obj.GetComponent<AudioSource>();
        Debug.Log($"Attempting {obj.name} to true");
        if (obj == null)
        {
            Debug.LogWarning("Object not assigned. Cannot function." );
            return;
        }
        obj.SetActive(true);
        audio.Play();
    }
}
