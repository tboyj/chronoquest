using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtTodo : MonoBehaviour
{
    public Transform target;
    public Transform player;
    public Camera playerCamera;
    private QuestManager qmRedundancySub;
    public bool invert = false;

    void Start()
    {
        qmRedundancySub = player.GetComponent<QuestManager>();
        
        // Auto-find camera if not assigned
        if (playerCamera == null)
            playerCamera = Camera.main;
    } 

    void Update()
    {
        QuestInstance currentQuest = qmRedundancySub?.GetCurrentQuest();
        
        // if (currentQuest == null || currentQuest.todo == null || currentQuest.todo.Count == 0)
        // {
        //     gameObject.transform.parent.parent.gameObject.SetActive(false);
        //     return;
        // } else
        // {
        //     gameObject.transform.parent.parent.gameObject.SetActive(true);
        // }
        // ^^ ==(work in progress lol)== //

        // Show the UI element
        gameObject.transform.parent.parent.gameObject.SetActive(true);
        
        // Get the target transform
        Transform todoTarget;
        if (currentQuest != null && currentQuest.todo.Count > 0)
        {
            todoTarget = currentQuest.todo[0].GetTodoAp();
            target = todoTarget;
        }
        if (target != null)
        {
            Vector3 directionToTarget = target.position - player.position;
            directionToTarget.y = 0; // Flatten to horizontal plane
            
            // Get camera's forward direction (flattened)
            Vector3 cameraForward = playerCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            
            // Get camera's right direction (flattened)
            Vector3 cameraRight = playerCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            
            // Project target direction onto camera plane
            float forwardDot = Vector3.Dot(directionToTarget.normalized, cameraForward);
            float rightDot = Vector3.Dot(directionToTarget.normalized, cameraRight);
            
            // Calculate angle relative to camera's forward direction
            float angle = Mathf.Atan2(rightDot, forwardDot) * Mathf.Rad2Deg;
            
            if (invert) angle += 180f;
            
            // Apply rotation to the UI element
            transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }
        // If we don't have a valid target, hide and return
        // if (target == null || player == null || playerCamera == null)
        // {
        //     gameObject.transform.parent.parent.gameObject.SetActive(false);
        //     return;
        // }

        // Get direction from player to target in world space
        
    }
}