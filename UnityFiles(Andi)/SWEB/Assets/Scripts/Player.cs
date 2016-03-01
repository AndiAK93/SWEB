using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    Deck deck_;
    Hand hand_;
    Field field_;
    Player enemy_;

    int health_;


	// Use this for initialization
	void Start () {
        deck_ = GetComponentInChildren<Deck>();
        hand_ = GetComponentInChildren<Hand>();
        field_ = GetComponentInChildren<Field>();
    }

    public Deck GetDeck() {
        return deck_;
    }

    public Hand GetHand() {
        return hand_;
    }

    public Field GetField() {
        return field_;
    }

    public void SetEnemy(Player enemy)
    {
        enemy_ = enemy;
    }

    public Player GetEnemy() {
        return enemy_;
    }

    public void ModifyHealth(int mod) {
        health_ += mod;
    }
}
