using System;
using UnityEngine;

public class PlayAnimation : MidQuestDialog
{
    public Animator animPlayback;
    public string animString;
    
    
    public override void ActionMidQuest()
    {
        Debug.Log("Playing animation override");
        if (animPlayback == null || obj == null)
        {
            Debug.LogWarning("Animator/object not assigned.");
            return;
        }
        // Make sure the clip is in the animation component
        // Play immediately instead of queued, unless you need sequential playback
        obj.SetActive(true);
        animPlayback.Play(animString, 0, 0f);
        // If you want to queue it instead:
        // animPlayback.PlayQueued(animation.name);
    }
}
