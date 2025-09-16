using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Gate
{
    void EnactCondition(bool trueOrFalse);
    IEnumerator waitingFunction();
}

public class PressActivationButton : ItemHandler , Gate
{
    public SpriteRenderer sprite;
    public bool amITurnedOn = false; // are you?
    [Range(0.5f, float.MaxValue)]
    public float duration;
    public Transform affectedObject;
    public enum ActivatedCondition { Gate, Button, Lever };
    public ActivatedCondition type;


    Gate gateRef;
    void Start()
    {
        gateRef = this;
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }
    
        void Gate.EnactCondition(bool trueOrFalse)
    {
        if (trueOrFalse)
        {
            sprite.color = Color.red;
            amITurnedOn = true;
            affectedObject.position = new Vector3(affectedObject.position.x, affectedObject.position.y + 3, affectedObject.position.z);
        }
        else
        {
            sprite.color = Color.green;
            amITurnedOn = false;
            affectedObject.position = new Vector3(affectedObject.position.x, affectedObject.position.y  - 3, affectedObject.position.z);
        }
    }
    IEnumerator Gate.waitingFunction()
    {
        Debug.Log("Step 1: Starting...");
        sprite.color = Color.red;
        amITurnedOn = true;
        gateRef.EnactCondition(amITurnedOn);
        yield return new WaitForSeconds(duration);
        Debug.Log("Step 2: After delay");
        sprite.color = Color.green;
        amITurnedOn = false;
        gateRef.EnactCondition(amITurnedOn);
    }

    protected override void HandleInteraction()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !amITurnedOn)
        {
            switch (type)
            {
                case ActivatedCondition.Gate:
                    StartCoroutine(gateRef.waitingFunction());
                    break;
            }
            
        }
    }


}