using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    public const int IDX_NAME_TEXT = 0;
    public const int IDX_DESCRIPTION_TEXT = 1;
    public const int IDX_ATTACK_TEXT = 2;
    public const int IDX_HEALTH_TEXT = 3;
    public const int IDX_DURATION_TEXT = 2;
    public const int IDX_MIN_ROUND_TEXT = 3;

    int id_;
    public string card_name_;
    string card_description_;

    int unique_id_;

    Text card_name_text_;
    Text card_description_text_;

    Player player_;
    CardLogic card_logic_;

    Image image_;
    public String image_name_ = "";

    public String image_left_ = "";

    public String image_right_ = "";

    bool is_on_hand_;
    bool being_dragged_ = false;

    void Awake() {
        card_name_text_ = GetComponentsInChildren<Text>()[Card.IDX_NAME_TEXT];
        //card_description_text_ = GetComponentsInChildren<Text>()[Card.IDX_DESCRIPTION_TEXT];

        Image[] images = GetComponentsInChildren<Image>();
        if (images.Length >= 2)
        {
            image_name_ = "card/card_front_lvk";
        }
        else {
            image_name_ = "card/card_front_wk_ak";
        }

        card_name_ = "";// card_logic_.GetType().ToString();
        card_description_ = "";
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void SetBackGroundImage(string path)
    {
        image_name_ = path;
    }

    public void SetImagePath(string path, string path2) {
        image_left_ = path;
        image_right_ = path2;
        card_logic_.RefreshVisuals();
    }


    public void LeftRewardPressed() {
        if (!Game.GetGame().IsMyTurn() || !IsMyCard()) return;

        //Network
        player_.EnemyLeftRewardPressed(GetUniqueId());

        Debug.Log("left");
        card_logic_.LeftReward();
    }

    public void RightRewardPressed() {
        if (!Game.GetGame().IsMyTurn() || !IsMyCard()) return;

        //Network
        player_.EnemyRightRewardPressed(GetUniqueId());

        Debug.Log("right");
        card_logic_.RightReward();
    }

    public void SetId(int id)
    {
        id_ = id;
    }

    public int GetId()
    {
        return id_;
    }

    public void SetUniqueId(int unique_id)
    {
        unique_id_ = unique_id;
    }

    public int GetUniqueId()
    {
        return unique_id_;
    }

    public void SetName(string new_name) {
        card_name_ = new_name;
        card_logic_.RefreshVisuals();
    }

    public void SetDescription(string desc)
    {
        card_description_ = desc;
    }

    public string GetName() {
        return card_name_;
    }

    public string GetDescription() {
        return card_description_;
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

    public CardLogic GetCardLogic()
    {
        return card_logic_;
    }

    public bool IsOnHand() {
        return is_on_hand_;
    }

    public bool CanBePutOnField() {
        return card_logic_.CanBePutOnField();
    }

    public void Update() {
        if (card_logic_ != null)
            card_logic_.Update();
    }

    public void PlayCard() {
        card_logic_.PlayCard();
    }

    public void UseOn(Card target) {
		playAttackSound ();
        ReturnType status = card_logic_.UseOn(target.card_logic_);

        //Network
        player_.EnemyUseOn(GetUniqueId(), target.GetUniqueId());

        if (status != ReturnType.NOT_POSSIBLE) {
            Debug.Log("Card " + GetName() + " is used on card " + target.GetName());
        }
    }

    public void UseOn(Player target)
    {		
        Debug.Log("Card " + name + " is used on Player " + target.name);
        ReturnType status = card_logic_.UseOn(target);

        if(target == player_)
        {
            player_.EnemyUseOnPlayer(GetUniqueId());
        }
        else
        {
            player_.GetEnemy().EnemyUseOnPlayer(GetUniqueId());
        }    

        if (status != ReturnType.NOT_POSSIBLE)
        {
			playAttackSound ();
            Debug.Log("Card " + name + " is used on Player " + target.name);
        }
    }

	private void playAttackSound()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		//AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load ("sound/attack2") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	} 



    public void Kill() {
        player_.GetHand().RemoveCardFromHand(this);
        player_.GetField().RemoveCardFromField(this);
        player_.GetDeck().RemoveCardFromDeck(this);
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
        if (!Game.GetGame().IsMyTurn() || !IsMyCard()) return;
        parent_to_return_to_ = new_parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!Game.GetGame().IsMyTurn() || !IsMyCard()) return;
        being_dragged_ = true;
        parent_to_return_to_ = this.transform.parent;
        drag_offset_.x = eventData.position.x - this.transform.position.x;
        drag_offset_.y = eventData.position.y - this.transform.position.y;
        this.transform.SetParent(this.transform.parent.parent.parent);
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!Game.GetGame().IsMyTurn() || !IsMyCard()) return;
        being_dragged_ = true;
        this.transform.position = eventData.position - drag_offset_;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // remove card from hand
        if (!Game.GetGame().IsMyTurn() || !IsMyCard()) return;
        being_dragged_ = false;
        this.transform.SetParent(parent_to_return_to_);
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Card dragged = eventData.pointerDrag.GetComponent<Card>();
        if (!Game.GetGame().IsMyTurn() || !dragged.IsMyCard()) return;
        if (dragged != null)
        {
            Card droped_onto = this;
            if (dragged != droped_onto && !droped_onto.IsOnHand() && (!dragged.IsOnHand() || (dragged.IsOnHand() && dragged.card_logic_.CanBeUsedFromHand())))
            {
                dragged.UseOn(droped_onto);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        color_ = GetComponentInChildren<Image>().color;
        GetComponentInChildren<Image>().color = color_highlight_;
        if((IsMyCard() || IsOnHand() && !being_dragged_) || (!IsMyCard() && !IsOnHand() && !being_dragged_))
        {
            Game.GetGame().GetInspector().ShowInspector();
            Game.GetGame().GetInspector().ShowCard(this);
        }
        //transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponentInChildren<Image>().color = color_;
        Game.GetGame().GetInspector().HideInspector();
        //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public bool IsMyCard()
    {
        PlayerServer playerServer = player_.GetComponent<PlayerServer>();
        PlayerClient playerClient = player_.GetComponent<PlayerClient>();

        if (playerClient != null && Network.peerType == NetworkPeerType.Client)
            return true;
        if (playerServer != null && Network.peerType == NetworkPeerType.Server)
            return true;
        return false;
    }
}
