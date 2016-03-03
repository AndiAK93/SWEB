using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Field : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    Player player_;
    public List<Card> cards_;

    

    // Use this for initialization
    void Start() {
        player_ = GetComponentInParent<Player>();
        cards_ = new List<Card>();
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnDrop(PointerEventData eventData) {
        Card card = eventData.pointerDrag.GetComponent<Card>();
       
        if (card != null && player_ == card.GetPlayer() && card.IsOnHand() && card.CanBePutOnField()) {
            if (!Game.GetGame().IsMyTurn() || !card.IsMyCard()) return;
            if (card.PlayCard() == ReturnType.OK)
            {
                GetComponent<NetworkView>().RPC("EnemyPlayCard", RPCMode.Others, card.GetUniqueId());
                Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
                card.SetParentToReturnTo(this.transform);
                player_.GetHand().RemoveCardFromHand(card);
                this.AddCardToField(card);
            }
        }
    }

	private void playDropSound(){
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		//AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load ("sound/click3") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	}

    [RPC]
    void EnemyPlayCard(int unique_id)
    {
        Card card = null;
        List<Card> hand_cards = player_.GetHand().cards_;
        for (int i = 0; i < hand_cards.Count; i++)
        {
            if (hand_cards[i].GetUniqueId() == unique_id) card = hand_cards[i];
        }

        if (card.PlayCard() == ReturnType.OK)
        {
            card.SetParentToReturnTo(this.transform);
            player_.GetHand().RemoveCardFromHand(card);

            // -
            Transform[] children = card.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(true);
            }
            card.GetComponent<LayoutElement>().preferredWidth = 180;
            card.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 150);
            Transform background = card.GetComponentInChildren<Transform>().Find("BackGround");
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 150);

            Sprite sprite = Resources.Load<Sprite>(card.image_name_);
            card.GetComponentInChildren<Image>().overrideSprite = sprite;
            // -

            this.AddCardToField(card);
        }

    }

    public void OnPointerEnter(PointerEventData eventData) {
    }

    public void OnPointerExit(PointerEventData eventData) {
    }

    public void UpdateCards() {
        for (int card_idx = 0; card_idx < cards_.Count; card_idx++) {
            cards_[card_idx].RefreshCard();
        }
    }

    public void AddCardToField(Card card) {
        Debug.Log("Added Card To Field " + card.GetName());
        card.gameObject.transform.SetParent(this.transform);
        card.gameObject.SetActive(true);
        card.SetOnHand(false);
        cards_.Add(card);
		playDropSound ();
    }

    public void RemoveCardFromField(Card card) {
        Debug.Log("Removed Card From Field " + card.GetName());
        cards_.Remove(card);
		playDestroySound ();
    }

	private void playDestroySound()
	{
		//AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/kill2") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	}

}
