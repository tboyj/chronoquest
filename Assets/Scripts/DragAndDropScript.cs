// using UnityEngine;
// using UnityEngine.EventSystems;

// public class DragAndDropScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// {
//     private Canvas canvas;
//     private RectTransform rectTransform;
//     private CanvasGroup canvasGroup;
//     private Vector3 originalPosition;
//     private Transform originalParent;

//     public Transform dragLayer;
//     private int thisIndex;
//     private int indexDraggedOn;

//     private Item thisItem;
//     private GameObject dragGhost;

//     void Awake()
//     {
//         canvas = GetComponentInParent<Canvas>();
//         rectTransform = GetComponent<RectTransform>();
//         canvasGroup = GetComponent<CanvasGroup>();
//         originalParent = transform.parent;
//     }

//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         thisIndex = originalParent.GetComponent<ItemGUI>().slotIndex;
//         thisItem = InventoryScript.instance.inventory[thisIndex];

//         if (thisItem?.item == null)
//         {
//             Debug.LogWarning("[BeginDrag] Tried to drag null item.");
//             return;
//         }

//         canvasGroup.blocksRaycasts = false;
//         originalPosition = rectTransform.position;

//         // Create ghost object for dragging
//         dragGhost = Instantiate(gameObject, dragLayer);
//         dragGhost.transform.SetAsLastSibling();
//         dragGhost.GetComponent<CanvasGroup>().blocksRaycasts = false;

//         Debug.Log($"[BeginDrag] Dragging item from slot {thisIndex}: {thisItem.item.name}");
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         if (dragGhost == null) return;

//         if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
//             canvas.transform as RectTransform,
//             eventData.position,
//             eventData.pressEventCamera,
//             out Vector3 worldPoint))
//         {
//             dragGhost.GetComponent<RectTransform>().position = worldPoint;
//         }
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         canvasGroup.blocksRaycasts = true;

//         DropIntoHolder[] dropZones = FindObjectsOfType<DropIntoHolder>();
//         DropIntoHolder targetZone = null;

//         foreach (DropIntoHolder zone in dropZones)
//         {
//             if (zone.isHovered)
//             {
//                 targetZone = zone;
//                 indexDraggedOn = zone.slotIndex;
//                 break;
//             }
//         }

//         Destroy(dragGhost);

//         if (targetZone == null)
//         {
//             Debug.LogWarning("[Drop] No valid drop zone found.");
//             ReturnToOriginal();
//             return;
//         }

//         if (indexDraggedOn == thisIndex)
//         {
//             Debug.LogWarning("[Swap] Dropped on same slot. Cancelling.");
//             ReturnToOriginal();
//             return;
//         }

//         // Swap items in inventory
//         Item replItem = InventoryScript.instance.inventory[indexDraggedOn];
//         InventoryScript.instance.inventory[indexDraggedOn] = thisItem;
//         InventoryScript.instance.inventory[thisIndex] = replItem;

//         // Refresh both slots
//         RefreshSlot(thisIndex);
//         RefreshSlot(indexDraggedOn);

//         Debug.Log($"[Inventory] Slot {thisIndex}: {InventoryScript.instance.inventory[thisIndex]?.item?.name ?? "null"}");
//         Debug.Log($"[Inventory] Slot {indexDraggedOn}: {InventoryScript.instance.inventory[indexDraggedOn]?.item?.name ?? "null"}");
//     }

//     private void ReturnToOriginal()
//     {
//         Debug.Log("[Return] No valid drop zone. Returned to original slot.");
//     }

//     private void RefreshSlot(int index)
//     {
//         foreach (DropIntoHolder zone in FindObjectsOfType<DropIntoHolder>())
//         {
//             if (zone.slotIndex == index)
//             {
//                 Item item = InventoryScript.instance.inventory[index];
//                 ItemGUI gui = zone.GetComponentInChildren<ItemGUI>();
//                 if (gui != null)
//                 {
//                     gui.UpdateSlotGUI(item);
//                     gui.RefreshFromInventory();

//                 }
//                 break;
//             }
//         }
//     }
// }