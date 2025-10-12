// using System;
// using System.Collections;
// using System.Collections.Generic;
// using ChronoQuest.Interactions.World;
// using ChronoQuest.Quests;
// using UnityEngine;

// public class BoneDustMinigame : MonoBehaviour, Interaction
// {
//     // Start is called before the first frame update

//     public QuestManager questManager;
//     // toaster handles it
//     [SerializeField]
//     private ItemStorable itemSought;
//     public bool inDialog { get; set; }
//     public StartBoneDust boneDustInit;
//     private NPC npcToRemember;



//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetKeyDown(Keybinds.actionKeybind) && questManager.GetComponent<Player>().GetHeldItem().item.id == itemSought.id)
//         {
//             if (questManager.GetComponent<Player>().holdingItemManager.GetActiveness())
//             {
//                 Debug.Log("Hello?");
//                 InteractionFunction();
//             }
//             else
//             {
//                 Debug.Log("not holding...");
//             }
//         }
//     }

//     // assign to a specific item's action that is obtained from being added to inventory (borrowed item) 
//     public void InteractionFunction()
//     {
//         if (questManager.GetCurrentQuest().data.id == transform.GetChild(0).GetComponent<DustingBonesQuest>().data.id)
//         // Debug here. Maybe its a problem with the way you get the child.
//         {
//             boneDustInit.Run();
//         }
//     }
    
// }

