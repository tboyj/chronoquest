using System.Collections;
using UnityEngine;

public class ButtonActivator : ItemHandler
{
    public SpriteRenderer sprite;
    public bool amITurnedOn = false;
    public bool timedActivater = false;
    [Range(0.5f, float.MaxValue)]
    public float duration = 1f;
    
    public Transform affectedObject;

    void Start()
    {
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }

    protected override void HandleInteraction()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !amITurnedOn)
        {
            if (timedActivater)
            {
                StartCoroutine(OpenGateRoutine());
                
            }
            else
            {
                sprite.color = Color.red;
                amITurnedOn = true; // infinite;
            }
        }
    }

    private IEnumerator OpenGateRoutine()
    {
        Debug.Log("Gate opening...");
        sprite.color = Color.red;
        amITurnedOn = true;

        affectedObject.position += Vector3.up * 3;

        yield return new WaitForSeconds(duration);

        Debug.Log("Gate closing...");
        sprite.color = Color.green;
        amITurnedOn = false;

        affectedObject.position += Vector3.down * 3;
    }
}