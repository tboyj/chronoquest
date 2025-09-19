using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropIntoHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public int slotIndex;
    public bool isHovered = false;
    public Transform currentHolder;

    // Start is called before the first frame update
    void Start()
    {
        // slotIndex = this.GetComponent<ItemGUI>().slotIndex;

        if (transform.childCount > 0)
        {
            currentHolder = transform.GetChild(0); // Assumes Holder is first child
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        Debug.Log("Hovering over slot: " + slotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        Debug.Log("End hovering over slot: " + slotIndex);
    }

}
