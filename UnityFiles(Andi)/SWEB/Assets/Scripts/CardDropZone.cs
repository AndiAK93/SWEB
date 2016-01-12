using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Color color_;
    private static Color color_highlight_ = Color.cyan;

    public void OnDrop(PointerEventData eventData)
    {
        Card dragged = eventData.pointerDrag.GetComponent<Card>();
        if (dragged != null) {
            Card droped_onto = GetComponent<Card>();
            if (dragged != droped_onto) {
                dragged.UseOn(droped_onto);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        color_ = GetComponent<Image>().color;
        GetComponent<Image>().color = color_highlight_;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = color_;
    }
}
