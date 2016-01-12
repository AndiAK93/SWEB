using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CardDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 drag_offset_;
    public Transform parent_to_return_to_ = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parent_to_return_to_ = this.transform.parent;
        drag_offset_.x = eventData.position.x - this.transform.position.x;
        drag_offset_.y = eventData.position.y - this.transform.position.y;
        this.transform.SetParent(this.transform.parent.parent.parent);
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position - drag_offset_;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parent_to_return_to_);
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
