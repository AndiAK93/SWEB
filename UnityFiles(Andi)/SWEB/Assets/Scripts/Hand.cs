using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Hand : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    Player player_;
    List<Card> cards_;

    // Use this for initialization
    void Start() {
        player_ = GetComponentInParent<Player>();
        cards_ = new List<Card>();
    }

    public void OnDrop(PointerEventData eventData) {
        /*Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
        CardDragger dragger = eventData.pointerDrag.GetComponent<CardDragger>();

        if (dragger != null)
        {
            dragger.parent_to_return_to_ = this.transform;
        }*/
    }

    public void OnPointerEnter(PointerEventData eventData) {
    }

    public void OnPointerExit(PointerEventData eventData) {
    }

    public void AddCardToHand(Card card) {
        Debug.Log("Added Card To Hand " + card.name);
        card.gameObject.transform.SetParent(this.transform);
        card.gameObject.SetActive(true);
        card.SetOnHand(true);
        cards_.Add(card);
    }

    public void RemoveCardFromHand(Card card) {
        Debug.Log("Removed Card From Hand " + card.name);
        card.SetOnHand(false);
        cards_.Remove(card);
    }
}
