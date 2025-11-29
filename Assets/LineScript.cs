using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxDistance = 100f;
    public LineRenderer lr;

    private void Update()
    {
        CastLaser();
    }

    void CastLaser()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        lr.positionCount = 1;
        lr.SetPosition(0, origin);

        for (int i = 0; i < 10; i++) // limit reflections so it doesn't go infinite
        {
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                // draw to hit point
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, hit.point);

                // reflect if mirror
                if (hit.collider.CompareTag("MirrorReflection"))
                {
                    // reflect direction
                    direction = Vector3.Reflect(direction, hit.normal);
                    origin = hit.point;
                }
                else
                {
                    // stop if not mirror
                    break;
                }
            }
            else
            {
                // draw end at max distance
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, origin + direction * maxDistance);
                break;
            }
        }
    }
}
