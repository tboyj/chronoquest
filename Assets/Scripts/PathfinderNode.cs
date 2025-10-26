using System.Collections.Generic;
using UnityEngine;

public class PathfinderNode : MonoBehaviour
{
    public bool wasVisited = false;
    public float nodeCost = 1f;
    public List<Transform> neighbors = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var n in neighbors)
        {
            if (n != null)
                Gizmos.DrawLine(transform.position, n.position);
        }
    }
}
