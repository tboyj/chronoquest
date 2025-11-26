using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFounder : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance = 50f;
    public LayerMask mask;
    [SerializeField]
    private GameObject reflection;
    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // Perform raycast (Editor only)
        RaycastHit hit;
        bool didHit = Physics.Raycast(origin, direction, out hit, distance, mask);

        Gizmos.color = didHit ? Color.green : Color.red;

        // Line to hit or full distance
        float drawDistance = didHit ? hit.distance : distance;

        Gizmos.DrawLine(origin, origin + direction * drawDistance);

        if (hit.collider.CompareTag("MirrorReflection")) {

            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask) && hit.collider.CompareTag("MirrorReflection"))
        {
            Debug.Log("Hit: " + hit.collider.name);
            
            // Example: draw line in scene view
            Debug.DrawLine(origin, hit.point, Color.green);
            reflection = hit.collider.transform.Find("rayshiner").gameObject;
            reflection.transform.position = hit.point;
            reflection.GetComponent<RaycastFounder>().distance = 50f;
            // Example: do something to the object
            // hit.collider.GetComponent<Something>()?.DoThing();
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * distance, Color.red);
        }
    }
}
