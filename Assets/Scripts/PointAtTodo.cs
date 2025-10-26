using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtTodo : MonoBehaviour
{
    public Transform target;
    public Transform player;
    private QuestManager qmRedundancySub; // reduces need to call each time
    public bool invert = false;       // Optional — flip direction

    void Start()
    {
        qmRedundancySub = player.GetComponent<QuestManager>();
        if (qmRedundancySub.GetCurrentQuest() == null)
        {
        }
    } 
    void Update()
    {
        if (qmRedundancySub.GetCurrentQuest() != null)
        {
            gameObject.transform.parent.parent.gameObject.SetActive(true);
            target = qmRedundancySub.GetCurrentQuest().todo[0].GetTodoAp();
        }
        if (target == null || player == null) return;

        // Get direction from player to target (in world space)
        Vector3 direction = target.position - player.position;

        // Convert that world direction into the player's local space
        // So the arrow behaves correctly even if the player rotates
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Apply the rotation to the UI element's Z axis (since UI uses 2D rotation)
        if (invert) angle += 180f;
        transform.rotation = Quaternion.Euler(0f, 0f, -angle);
    }
}
