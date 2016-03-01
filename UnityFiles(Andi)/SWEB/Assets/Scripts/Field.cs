using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Field : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    Player player_;
    List<Card> cards_;

    // Use this for initialization
    void Start() {
        player_ = GetComponentInParent<Player>();
        cards_ = new List<Card>();
    }

    public void OnDrop(PointerEventData eventData) {
        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (card != null && player_ == card.GetPlayer() && card.IsOnHand() && card.CanBePutOnField()) {
            Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
            card.SetParentToReturnTo(this.transform);
            player_.GetHand().RemoveCardFromHand(card);
            this.AddCardToField(card);
            card.PlayCard();
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
    }

    public void OnPointerExit(PointerEventData eventData) {
    }

    public void UpdateCards() {
        for (int card_idx = 0; card_idx < cards_.Count; card_idx++) {
            cards_[card_idx].Update();
        }
    }

    public void AddCardToField(Card card) {
        Debug.Log("Added Card To Field " + card.GetName());
        card.gameObject.transform.SetParent(this.transform);
        card.gameObject.SetActive(true);
        card.SetOnHand(false);
        cards_.Add(card);
    }

    public void RemoveCardFromField(Card card) {
        Debug.Log("Removed Card From Field " + card.GetName());
        cards_.Remove(card);
    }
}
