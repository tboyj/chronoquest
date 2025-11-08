using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public GameObject positionTP;
    public Vector3 teleportToPosition;
    public void Start()
    {
        teleportToPosition = positionTP.transform.position;
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
            other.GetComponent<QuestManager>().GetCurrentQuest().QuestEventTriggered();
        } else if (other.CompareTag("NPC"))
        {
            Teleport(other.gameObject);
        }
    }
}
