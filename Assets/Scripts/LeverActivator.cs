using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeverActivator : ItemHandler
{
    public SpriteRenderer sprite;
    public bool toggled = false;
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
            toggled = !toggled;
            leverCheck();
        }
        }

    void leverCheck()
    {
        if (toggled)
        {
            Debug.Log("Gate opening...");
            sprite.color = Color.red;
            affectedObject.position = new Vector3(affectedObject.position.x, (affectedObject.position.y + 3), affectedObject.position.z); //Vector3.up * 3;
        }
        else
        {
            Debug.Log("Gate closing...");
            sprite.color = Color.green;
            affectedObject.position = new Vector3(affectedObject.position.x, (affectedObject.position.y - 3), affectedObject.position.z);
        }
            // +5 on each as a shift up since the baseplate is set at 5
        }
    }
