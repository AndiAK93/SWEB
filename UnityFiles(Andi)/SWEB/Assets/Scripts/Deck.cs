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
            Card card = CreateCard(card_idx % 2 + 1);
            cards_.Add(card);
        }
    }

    public Card CreateCard(int id) {
        GameObject new_card = null;
        Card card = null;
        switch (id % 3) {
            case 0:
                new_card = Instantiate(card_init_knowledge_);
                card = new_card.GetComponent<Card>();
                card.SetCardLogic(new CardKnowledge(card, 5, 6, null));
                break;
            case 1:
                new_card = Instantiate(card_init_activity_);
                card = new_card.GetComponent<Card>();
                card.SetCardLogic(new CardActivity(card, new EffectIncHealth()));
                break;
            case 2:
                new_card = Instantiate(card_init_lecture_);
                card = new_card.GetComponent<Card>();
                card.SetCardLogic(new CardLecture(card, 1, 1, new EffectSpawnCard(), null));
                break;
        }

        new_card.transform.SetParent(this.transform);
        new_card.transform.position = this.transform.position;
        new_card.name = "Card " + cards_.Count.ToString();
        new_card.SetActive(false);
        card.SetPlayer(player_);
        return card;
    }

    public void DrawCardFromDeck() {
        if (cards_.Count > 0) {
            Card card = cards_[0];
            cards_.RemoveAt(0);
            player_.GetHand().AddCardToHand(card);
        }
    }

    public void RemoveCardFromDead(Card card) {
        cards_.Remove(card);
    }
}
