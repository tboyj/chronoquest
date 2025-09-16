using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressActivationButton : ItemHandler
{
    public SpriteRenderer sprite;
    public bool amITurnedOn = false; // are you?
    [Range(0.5f, float.MaxValue)]
    public float duration;
    public String typeOfCondition;
    public Transform affectedObject;
    void Start()
    {
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }
    protected override void HandleInteraction()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(conditionRuntime());

        }
    }
    IEnumerator conditionRuntime()
    {
        Debug.Log("Step 1: Starting...");
        sprite.color = Color.red;
        amITurnedOn = true;
        doCondition(typeOfCondition, amITurnedOn);
        yield return new WaitForSeconds(duration);
        Debug.Log("Step 2: After delay");
        sprite.color = Color.green;
        amITurnedOn = false;
        doCondition(typeOfCondition, amITurnedOn);
    }
    IEnumerator gate(bool tof)
    {

        float direction = tof ? 0.05f : -0.05f;
        yield return new WaitForSeconds(.05f);
        affectedObject.position += new Vector3(0, direction, 0);
    }
    public void doCondition(String condition, bool mode)
    {

        switch (typeOfCondition)
        {
            case "gate":
                activateGate(mode);
                break;
        }
        // do a reverse condition
    }

    private void activateGate(bool tof)
    {
        for (int i = 0; i < 40; i++)
        {
            StartCoroutine(gate(tof));
        }
    }
}