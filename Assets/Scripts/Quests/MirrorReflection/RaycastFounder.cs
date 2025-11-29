using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFounder : MonoBehaviour
{
    // Start is called before the first frame update
    public bool activeReflection;
    public float distance = 50f;
    public bool hitByPreviousMirror = false;
    public LayerMask mask;
    [SerializeField]
    private RaycastFounder reflection;
    public List<Collider> mirrorChain = new List<Collider>();
    void Start()
    {
        LookingForReflections();
    }
    void Update()
    {
        LookingForReflections();
    }

  public void LookingForReflections()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask))
        {

            // Debug.Log("Hit: " + hit.collider.name);
            if (hit.collider.CompareTag("MirrorReflection"))
            {
                 // Track the hit
                activeReflection = true;
                if (hit.collider.transform?.Find("rayshiner")?.gameObject != null) {
                    reflection = hit.collider.transform.Find("rayshiner").gameObject.GetComponent<RaycastFounder>();
                    // reflection.transform.position = hit.point; caused too much vulnerability for reflections. if refined this would be propr
                    reflection.distance = 50f;
                    reflection.hitByPreviousMirror = true;
                }
                
            // Example: do something to the object
            // hit.collider.GetComponent<Something>()?.DoThing();
            } else if (hit.collider.CompareTag("RecieverForReflection"))
            {
                Debug.DrawLine(origin, hit.point, Color.green);
                hit.collider.GetComponent<ReflectionReciever>().CheckAllReflections();
                activeReflection = true;
            }
            else
            {
                activeReflection = false;
                if (reflection != null)
                    reflection.hitByPreviousMirror = false;
            }
            
            if (activeReflection && hitByPreviousMirror)
            {
                Debug.DrawLine(origin, hit.point, Color.green);
            } else
            {
                Debug.DrawLine(origin, origin + direction * distance, Color.red);
            }
        }
        else
        {
            
        }
    }
}
