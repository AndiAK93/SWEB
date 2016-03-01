using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    public const int IDX_NAME_TEXT = 0;
    public const int IDX_DESCRIPTION_TEXT = 1;
    public const int IDX_ATTACK_TEXT = 2;
    public const int IDX_HEALTH_TEXT = 3;
    public const int IDX_DURATION_TEXT = 2;
    public const int IDX_MIN_ROUND_TEXT = 3;

    int id_;
    string card_name_;
    string card_description_;

    Text card_name_text_;
    Text card_description_text_;

    Player player_;
    CardLogic card_logic_;

    bool is_on_hand_;

    void Start() {
        card_name_text_ = GetComponentsInChildren<Text>()[Card.IDX_NAME_TEXT];
        card_description_text_ = GetComponentsInChildren<Text>()[Card.IDX_DESCRIPTION_TEXT];
        card_name_text_.text = card_logic_.GetType().ToString();
        card_description_text_.text = "Description";
    }

    public string GetName() {
        return card_logic_.GetType().ToString();
    }

    public string GetDescription() {
        return "Hier steht die beschreiebung + Efffect etc";
    }

    public void SetPlayer(Player player) {
        player_ = player;
    }

    public Player GetPlayer() {
        return player_;
    }

    public void SetOnHand(bool on_hand) {
        is_on_hand_ = on_hand;
    }

    public void SetCardLogic(CardLogic logic) {
        card_logic_ = logic;
    }

    public bool IsOnHand() {
        return is_on_hand_;
    }

    public bool CanBePutOnField() {
        return card_logic_.CanBePutOnField();
    }

    public void Update() {
        card_logic_.Update();
    }

    public void PlayCard() {
        card_logic_.PlayCard();
    }

    public void UseOn(Card target) {
        ReturnType status = card_logic_.UseOn(target.card_logic_);
        if (status != ReturnType.NOT_POSSIBLE) {
            Debug.Log("Card " + name + " is used on card " + target.name);
        }
    }

    public void UseOn(Player target)
    {
        Debug.Log("Card " + name + " is used on Player " + target.name);
        ReturnType status = card_logic_.UseOn(target);
        if (status != ReturnType.NOT_POSSIBLE)
        {
            Debug.Log("Card " + name + " is used on Player " + target.name);
        }
    }

    public void Kill() {
        player_.GetHand().RemoveCardFromHand(this);
        player_.GetField().RemoveCardFromField(this);
        player_.GetDeck().RemoveCardFromDead(this);
        Destroy(this.gameObject);
    }

    /**
    The following functions belong to the interfaces we use 
    **/
    Vector2 drag_offset_;
    Transform parent_to_return_to_ = null;
    Color color_;
    static Color color_highlight_ = Color.cyan;

    public void SetParentToReturnTo(Transform new_parent) {
        parent_to_return_to_ = new_parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        parent_to_return_to_ = this.transform.parent;
        drag_offset_.x = eventData.position.x - this.transform.position.x;
        drag_offset_.y = eventData.position.y - this.transform.position.y;
        this.transform.SetParent(this.transform.parent.parent.parent);
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        this.transform.position = eventData.position - drag_offset_;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // remove card from hand
        this.transform.SetParent(parent_to_return_to_);
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData) {
        Card dragged = eventData.pointerDrag.GetComponent<Card>();
        if (dragged != null) {
            Card droped_onto = this;
            if (dragged != droped_onto && !droped_onto.IsOnHand() && (!dragged.IsOnHand() || (dragged.IsOnHand() && dragged.card_logic_.CanBeUsedFromHand()))) {
                dragged.UseOn(droped_onto);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        color_ = GetComponentInChildren<Image>().color;
        GetComponentInChildren<Image>().color = color_highlight_;
        Game.GetGame().GetInspector().ShowInspector();
        Game.GetGame().GetInspector().ShowCard(this);
        //transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponentInChildren<Image>().color = color_;
        Game.GetGame().GetInspector().HideInspector();
        //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
