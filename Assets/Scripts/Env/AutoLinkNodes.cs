using UnityEngine;

public class AutoLinkNodes : MonoBehaviour
{
    public float connectionDistance = 5f;

    void Awake()
    {
        PathfinderNode[] nodes = FindObjectsOfType<PathfinderNode>();

        foreach (PathfinderNode node in nodes)
        {
            node.neighbors.Clear();

            foreach (PathfinderNode other in nodes)
            {
                if (other == node) continue;
                if (Vector3.Distance(node.transform.position, other.transform.position) <= connectionDistance)
                {
                    node.neighbors.Add(other.transform);
                }
            }
        }

        Debug.Log("Auto-linked all nearby nodes!");
    }
}
