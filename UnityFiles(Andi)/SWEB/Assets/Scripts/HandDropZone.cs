using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HandDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
        CardDragger dragger = eventData.pointerDrag.GetComponent<CardDragger>();
        if (dragger != null)
        {
            dragger.parent_to_return_to_ = this.transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
