using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : QuestInstance
{
    public GameObject positionTP;
    public Vector3 teleportToPosition;
    public void Start()
    {

        teleportToPosition = positionTP.transform.position;
    }

    public void Teleport(GameObject target)
        {
            PlayerMovement movement = target.GetComponent<PlayerMovement>(); // or whatever your movement script is called
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
            questManager.SetQuestCompleted(questManager.GetCurrentQuest());
            Teleport(other.gameObject);
            if (gameObject.GetComponent<ExtraBase>() != null) // activate extra behavior if there is any
                {
                    gameObject.GetComponent<ExtraBase>().Change();
                }
            other.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        } else if (other.CompareTag("NPC"))
        {
            Teleport(other.gameObject);
        }
    }

}
