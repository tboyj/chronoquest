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
        if (qmRedundancySub.GetCurrentQuest() != null)
        {
            gameObject.transform.parent.parent.gameObject.SetActive(true);
            target = qmRedundancySub.GetCurrentQuest().todo[0].GetTodoAp();
        }
        else
        {
            gameObject.transform.parent.parent.gameObject.SetActive(false);
            return;
        }
        
        if (target == null || player == null || playerCamera == null) return;

        // Get direction from player to target in world space
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
}