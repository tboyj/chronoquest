using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMovement : Movement
{
    public string status;
    public GameObject pathPointContainer;
    public List<Transform> pathPoints = new List<Transform>();
    public Transform currentNode;
    public Transform endNode;

    private Queue<Transform> currentPath = new Queue<Transform>();
    private float minDist = Mathf.Infinity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("CharacterController missing on NPC.");

        pathPointContainer = GameObject.Find("AINodeHolder");

        foreach (Transform child in pathPointContainer.transform)
            pathPoints.Add(child);

        currentNode = GetClosestNode();
        endNode = GameObject.FindWithTag("End").transform;

        if (currentNode && endNode)
            SetPathToEnd();
    }

    private void Update()
    {
        if (status != "Idle")
            MoveWithForce();
    }

    public override void MoveWithForce()
    {
        if (Time.timeScale <= 0) return;

        if (currentNode == null && currentPath.Count == 0) return;

        // If reached current node, move to next
        float dist = Vector3.Distance(transform.position, currentNode.position);
        if (dist < 0.5f)
        {
            if (currentPath.Count > 0)
                currentNode = currentPath.Dequeue();
            else
                return; // reached end
        }

        // Move toward current node
        Vector3 dir = (currentNode.position - transform.position).normalized;
        Vector3 move = new Vector3(dir.x, 0, dir.z) * moveSpeed * Time.deltaTime;

        controller.Move(move);

        // Gravity
        if (!controller.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = -2f;

        controller.Move(velocity * Time.deltaTime);

        // Flip NPC
        if (dir.x < 0) flip = true;
        else if (dir.x > 0) flip = false;
    }

    private void SetPathToEnd()
    {
        List<Transform> path = FindShortestPath(currentNode, endNode);
        currentPath = new Queue<Transform>(path);
    }

    // --- Shortest Path Finder (A*) ---
    public List<Transform> FindShortestPath(Transform start, Transform goal)
    {
        var openSet = new List<Transform> { start };
        var cameFrom = new Dictionary<Transform, Transform>();

        var gScore = new Dictionary<Transform, float>();
        var fScore = new Dictionary<Transform, float>();

        foreach (Transform node in pathPoints)
        {
            gScore[node] = Mathf.Infinity;
            fScore[node] = Mathf.Infinity;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Transform current = GetLowestFScore(openSet, fScore);
            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (Transform neighbor in current.GetComponent<PathfinderNode>().neighbors)
            {
                float tentativeG = gScore[current] + Vector3.Distance(current.position, neighbor.position) * neighbor.GetComponent<PathfinderNode>().nodeCost;
                if (tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Transform>(); // no path found
    }

    private float Heuristic(Transform a, Transform b)
    {
        float dist = Vector3.Distance(a.position, b.position);
        float bonus = 0f;
        if (a.CompareTag("Path"))
            bonus = -10f;
        
        return dist + bonus;
    }

    private Transform GetLowestFScore(List<Transform> openSet, Dictionary<Transform, float> fScore)
    {
        Transform best = openSet[0];
        float lowest = fScore[best];

        foreach (Transform node in openSet)
        {
            if (fScore[node] < lowest)
            {
                lowest = fScore[node];
                best = node;
            }
        }
        
        return best;
    }

    private List<Transform> ReconstructPath(Dictionary<Transform, Transform> cameFrom, Transform current)
    {
        var totalPath = new List<Transform> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        totalPath.RemoveAt(0); // remove starting node
        return totalPath;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            foreach (Transform child in pathPointContainer.transform)
                child.GetComponent<PathfinderNode>().wasVisited = false;

            SetPathToEnd(); // recalculate
        }
    }

    public Transform GetClosestNode()
    {
        Transform minNode = null;
        float minDistLocal = Mathf.Infinity;

        foreach (Transform node in pathPoints)
        {
            if (node != null)
            {
                float dist = Vector3.Distance(transform.position, node.position);
                if (dist < minDistLocal)
                {
                    minDistLocal = dist;
                    minNode = node;
                }
            }
        }

        return minNode;
    }
}
