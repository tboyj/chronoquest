// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
// public class LineRendererController : MonoBehaviour
// {
//     [Header("Ray Settings")]
//     public float maxDistance = 50f;
//     public int maxReflections = 10;
//     public LayerMask mask;

//     [Header("References")]
//     public LineRenderer lineRenderer;

//     // Persistent list of mirrors active in previous frame
//     private List<GameObject> activeMirrors = new List<GameObject>();

//     private void Start()
//     {
//         if (lineRenderer == null)
//             lineRenderer = GetComponent<LineRenderer>();

//         lineRenderer.positionCount = 0;
//         lineRenderer.startWidth = 0.1f;
//         lineRenderer.endWidth = 0.1f;
//         lineRenderer.enabled = true;
//     }

//     private void Update()
//     {
//         DrawManualReflection();
//     }

//     private void DrawManualReflection()
//     {
//         Vector3 origin = transform.position;
//         Vector3 direction = transform.forward;

//         List<Vector3> points = new List<Vector3>();
//         points.Add(origin);

//         // Track mirrors hit in this frame
//         List<Collider> hitMirrors = new List<Collider>();

//         // Disable mirrors from last frame
//         foreach (var mirror in activeMirrors)
//         {
//             if (mirror != null)
//                 mirror.SetActive(false);
//         }
//         activeMirrors.Clear();

//         for (int i = 0; i < maxReflections; i++)
//         {
//             if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, mask))
//             {
//                 points.Add(hit.point);

//                 if (hit.collider.CompareTag("MirrorReflection"))
//                 {
//                     if (hitMirrors.Contains(hit.collider))
//                         break;

//                     hitMirrors.Add(hit.collider);

//                     // Enable this mirror as it’s part of the chain
//                     hit.collider.gameObject.SetActive(true);
//                     activeMirrors.Add(hit.collider.gameObject);

//                     Transform rayShiner = hit.collider.transform.Find("rayshiner");
//                     if (rayShiner != null)
//                         direction = rayShiner.forward;

//                     origin = hit.point;
//                 }
//                 else if (hit.collider.CompareTag("ReceiverForReflection"))
//                 {
//                     hit.collider.GetComponent<ReflectionReciever>()?.CheckAllReflections();
//                     break;
//                 }
//                 else
//                 {
//                     // hit a wall or obstacle → stop chain
//                     points.Add(hit.point);
//                     break;
//                 }

//             }
//             else
//             {
//                 points.Add(origin + direction * maxDistance);
//                 break;
//             }
//         }

//         lineRenderer.positionCount = points.Count;
//         lineRenderer.SetPositions(points.ToArray());
//     }
// }
