using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOfLight : MonoBehaviour
{
    public LayerMask mask;
    public float distance = 50f;
    public int maxBounces = 10;

    private LineRenderer lr;
    private List<Vector3> points = new List<Vector3>();

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (lr == null)
            lr = gameObject.GetComponent<LineRenderer>();

        lr.positionCount = 0;
        lr.widthMultiplier = 0.1f;
        lr.startColor = lr.endColor = Color.white;
    }

    void Update()
    {
        points.Clear();
        CastLight(transform.position, transform.forward, maxBounces);

        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }

    void CastLight(Vector3 origin, Vector3 direction, int bounces)
    {
        if (bounces <= 0) return;

        points.Add(origin);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask))
        {
            points.Add(hit.point);

            if (hit.collider.CompareTag("MirrorReflection"))
            {
                Vector3 reflected = Vector3.Reflect(direction, hit.normal);
                // play sound once!
                CastLight(hit.point, reflected, bounces - 1);
            }
            else if (hit.collider.CompareTag("RecieverForReflection"))
            {
                hit.collider.GetComponent<ReflectionReciever>()?.CheckAllReflections();
            }
        }
        else
        {
            points.Add(origin + direction * distance);
        }
    }
}
