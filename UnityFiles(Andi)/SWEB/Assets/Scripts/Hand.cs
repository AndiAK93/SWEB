using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Hand : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    Player player_;
    public List<Card> cards_;

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
        Debug.Log("Added Card To Hand " + card.GetName());
        card.gameObject.transform.SetParent(this.transform);

        if (!card.IsMyCard())
        {
            Transform[] children = card.GetComponentsInChildren<Transform>();
            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(false);
            }
            card.GetComponent<LayoutElement>().preferredWidth = 110;
            card.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 150);
            Transform background = card.GetComponentInChildren<Transform>().Find("BackGround");
            background.gameObject.SetActive(true);
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 150);

            Sprite sprite = Resources.Load<Sprite>("card/card_back_b_t");
            card.GetComponentInChildren<Image>().overrideSprite = sprite;
        }

        card.gameObject.SetActive(true);
        card.SetOnHand(true);
        cards_.Add(card);
    }

    public void RemoveCardFromHand(Card card) {
        Debug.Log("Removed Card From Hand " + card.GetName());
        card.SetOnHand(false);
        cards_.Remove(card);
    }


}
