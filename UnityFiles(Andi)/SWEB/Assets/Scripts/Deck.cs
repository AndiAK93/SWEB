﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Deck : MonoBehaviour {
    public GameObject card_init_knowledge_;
    public GameObject card_init_lecture_;
    public GameObject card_init_activity_;

    Player player_;
    public List<Card> cards_;

    class Constants {
        public static int CardsPerDeck = 10;
    }

    // Use this for initialization
    void Start() {
        player_ = GetComponentInParent<Player>();
        cards_ = new List<Card>();
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void CreateRandomDeck() {
        Debug.Log("CREATING RANDOM DECKKKSKSKKSKSKSKK");
        for (int card_idx = 0; card_idx < Constants.CardsPerDeck; card_idx++) {
            card_t db_Card = Game.GetGame().GetDataBank().getRandomCard();
            GetComponent<NetworkView>().RPC("GeneratedCardForDeck", RPCMode.Others, db_Card.id_);
            cards_.Add(CreateCard(db_Card));
        }
    }

    [RPC]
    private void GeneratedCardForDeck(int id)
    {
        card_t db_Card = Game.GetGame().GetDataBank().getCard(id);
        cards_.Add(CreateCard(db_Card));
    }

    public Card CreateCard(card_t db_card) {
        GameObject new_card = null;
        Card card = null;

        switch (db_card.cardType_) {
            case (int)CardType.Knowledge:
                new_card = Instantiate(card_init_knowledge_);
                card = new_card.GetComponent<Card>();
                knowledgecard_t k = (knowledgecard_t)db_card;
                card.SetCardLogic(new CardKnowledge(card, k.attack_, k.defense_, Effect.CreateEffect(k.effectType_, k.effectValue_)));
                card.SetImagePath(db_card.image_, "");
                break;

            case (int)CardType.Activity:
                new_card = Instantiate(card_init_activity_);
                card = new_card.GetComponent<Card>();
                actionCard_t a = (actionCard_t)db_card;
                card.SetCardLogic(new CardActivity(card, a.cost_, Effect.CreateEffect(a.effectType_, a.effectValue_)));
                card.SetImagePath(db_card.image_, "");
                card.SetDescription(a.description_);
                break;

            case (int)CardType.Lecture:
                new_card = Instantiate(card_init_lecture_);
                card = new_card.GetComponent<Card>();
                LVCard_t l = (LVCard_t)db_card;

                card_t c1 = Game.GetGame().GetDataBank().getCard(l.CardRewardID_[0]);
                card_t c2 = Game.GetGame().GetDataBank().getCard(l.CardRewardID_[1]);

                card.SetCardLogic(new CardLecture(card, l.startRound_, l.duration_, new EffectSpawnCard(l.CardRewardID_[0]), new EffectSpawnCard(l.CardRewardID_[1])));
                string[] image_names = db_card.image_.Split('-');
                card.SetImagePath(image_names[0], image_names[1]);

                string desc = "Left Reward: " + ((c1.cardType_ == (int)CardType.Activity) ? ((actionCard_t)c1).description_ : "") + "\n" + "Right Reward: " + ((c2.cardType_ == (int)CardType.Activity) ? ((actionCard_t)c2).description_ : "");


                /*string desc =   "Left Reward: " + ((c1.cardType_ == (int)CardType.Activity) ? ((actionCard_t)c1).description_ : ("Attack: " + ((knowledgecard_t)c1).attack_.ToString() + " Health: " + ((knowledgecard_t)c1).defense_.ToString())) + "\n" +
                                "Right Reward: " + ((c2.cardType_ == (int)CardType.Activity) ? ((actionCard_t)c2).description_ : ("Attack: " + ((knowledgecard_t)c2).attack_.ToString() + " Health: " + ((knowledgecard_t)c2).defense_.ToString()));
                */
                card.SetDescription(desc);
           
                break;

            default:
                Debug.Log("Invalid Card Type");
                Debug.Log("" + db_card.id_.ToString());
                break;
        }

        new_card.transform.SetParent(this.transform);
        new_card.transform.position = this.transform.position;
        new_card.name = db_card.name_;
        new_card.SetActive(false);
        card.SetId(db_card.id_);
        card.SetUniqueId(Game.unique_card_id++);
        card.SetPlayer(player_);
        card.SetName(db_card.name_);
        return card;
    }

    public void DrawCardFromDeck() {
        if (!Game.GetGame().IsMyTurn()) return;
        if (Game.GetGame().canDrawCard) Game.GetGame().canDrawCard = false;
        else return; 

        Draw();

		Game.GetGame().playDrawSound ();
        GetComponent<NetworkView>().RPC("Draw", RPCMode.Others);
    }



    public void InitialDrawCardFromDeck()
    {
        Draw();
        GetComponent<NetworkView>().RPC("Draw", RPCMode.Others);
    }

    [RPC]
    public void Draw()
    {
        if (cards_.Count > 0)
        {
            Card card = cards_[0];
            cards_.RemoveAt(0);
            player_.GetHand().AddCardToHand(card);
        }
    }


    public void RemoveCardFromDeck(Card card) {
        cards_.Remove(card);
    }
}
