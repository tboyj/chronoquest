using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : QuestInstance
{
    public Vector3 teleportToPosition;

    public TeleportScript(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s) : base(q, i, t, d, s)
    {
        
    }

    public void Teleport(GameObject target)
        {
            var movement = target.GetComponent<PlayerMovement>(); // or whatever your movement script is called
            if (movement != null && movement.controller != null)
            {
                movement.controller.enabled = false;
                target.transform.position = teleportToPosition;
                movement.controller.enabled = true;
            }
            else
            {
                Debug.LogWarning("Teleport failed: movement or controller missing.");
            }
        }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Teleport(other.gameObject);
            other.GetComponent<QuestManagerGUI>().GotoNextTodo();
            QuestEventTriggered();
        } else if (other.CompareTag("NPC"))
        {
            Teleport(other.gameObject);
        }
    }
}
