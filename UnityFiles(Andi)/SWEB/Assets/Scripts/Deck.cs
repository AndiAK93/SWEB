using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Deck : MonoBehaviour {
    public GameObject card_init_knowledge_;
    public GameObject card_init_lecture_;
    public GameObject card_init_activity_;

    Player player_;
    List<Card> cards_;

    class Constants {
        public static int CardsPerDeck = 10;
    }

    // Use this for initialization
    void Start() {
        player_ = GetComponentInParent<Player>();
        cards_ = new List<Card>();
    }

    public void CreateRandomDeck() {
        for (int card_idx = 0; card_idx < Constants.CardsPerDeck; card_idx++) {
            card_t db_Card = Game.GetGame().GetDataBank().getRandomCard();
            cards_.Add(CreateCard(db_Card));
        }
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
                break;

            case (int)CardType.Activity:
                new_card = Instantiate(card_init_activity_);
                card = new_card.GetComponent<Card>();
                actionCard_t a = (actionCard_t)db_card;
                card.SetCardLogic(new CardActivity(card, a.cost_, Effect.CreateEffect(a.effectType_, a.effectValue_)));
                break;

            case (int)CardType.Lecture:
                new_card = Instantiate(card_init_lecture_);
                card = new_card.GetComponent<Card>();
                LVCard_t l = (LVCard_t)db_card;
                card.SetCardLogic(new CardLecture(card, l.startRound_, l.duration_, new EffectSpawnCard(l.CardRewardID_[0]), null));
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
        card.SetPlayer(player_);
        card.SetName(db_card.name_);
        card.UpdateName();
        
        return card;
    }

    public void DrawCardFromDeck() {
        if (cards_.Count > 0) {
            Card card = cards_[0];
            cards_.RemoveAt(0);
            player_.GetHand().AddCardToHand(card);
        }
    }

    public void RemoveCardFromDeck(Card card) {
        cards_.Remove(card);
    }
}
