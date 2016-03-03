using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class PlayerAttackZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    Player player_;
    Color color_;
    static Color color_highlight_ = Color.cyan;

    public void OnDrop(PointerEventData eventData)
    {
        Card card = eventData.pointerDrag.GetComponent<Card>();

        if (card != null)
        {
            if (!Game.GetGame().IsMyTurn() || !card.IsMyCard()) return;
            //GetComponent<NetworkView>().RPC("EnemyPlayCard", RPCMode.Others, card.GetUniqueId());
            Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
            card.UseOn(player_);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        color_ = GetComponentInChildren<Image>().color;
        GetComponentInChildren<Image>().color = color_highlight_;
        //Game.GetGame().GetInspector().ShowInspector();
        //Game.GetGame().GetInspector().ShowCard(this);
        //transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<Image>().color = color_;
        //Game.GetGame().GetInspector().HideInspector();
        //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }


    // Use this for initialization
    void Start () {
        player_ = GetComponentInParent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
