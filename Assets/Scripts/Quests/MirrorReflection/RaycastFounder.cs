using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFounder : MonoBehaviour
{
public int id;
public float distance = 50f;
public LayerMask mask;
public bool bigPapa;


[HideInInspector]  
public bool activeReflection;  
[HideInInspector]  
public bool hitByPreviousMirror;  

private RaycastFounder reflection;  
private static List<RaycastFounder> mirrorChain = new List<RaycastFounder>();  

void Awake()  
{  
    if (!mirrorChain.Contains(this))  
        mirrorChain.Add(this);  

    // Sort chain once by ID  
    mirrorChain.Sort((a, b) => a.id.CompareTo(b.id));  
}  

void Update()  
{  
    // Update mirrors in order to propagate reflections correctly  
    foreach (var mirror in mirrorChain)  
        mirror.ProcessReflection();  
}  

void ProcessReflection()  
{  
    activeReflection = false;  

    Vector3 origin = transform.position;  
    Vector3 direction = transform.forward;  

    if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask))  
    {  
        // Mirror hit  
        if (hit.collider.CompareTag("MirrorReflection"))  
        {  
            

            var rayshiner = hit.collider.transform?.Find("rayshiner")?.GetComponent<RaycastFounder>();  
            if (rayshiner != null && rayshiner.id > id)  
            {  
                activeReflection = true;  
                reflection = rayshiner;  
                reflection.distance = 50f;  
            }  
        }  
        // Receiver hit  
        else if (hit.collider.CompareTag("RecieverForReflection"))  
        {  
            hit.collider.GetComponent<ReflectionReciever>()?.CheckAllReflections();  
            activeReflection = true;  
        }  

        // Update chain hit status  
        hitByPreviousMirror = bigPapa || CheckPreviousMirrors();  

        // Draw debug lines  
        Color lineColor = hitByPreviousMirror  
            ? (activeReflection ? Color.green : Color.blue)  
            : Color.red;  

        Debug.DrawLine(origin, hit.point, lineColor);  
    }  
    else  
    {  
        // No hit line  
        Debug.DrawLine(origin, origin + direction * distance, Color.red);  
        hitByPreviousMirror = false;  
    }  
}  

bool CheckPreviousMirrors()  
{  
    for (int i = 0; i < mirrorChain.Count; i++)  
    {  
        if (mirrorChain[i] == this) break;  
        if (!mirrorChain[i].activeReflection) return false;  
    }  
    return true;  
}  


}
