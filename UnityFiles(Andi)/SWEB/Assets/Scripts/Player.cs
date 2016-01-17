using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    Deck deck_;
    Hand hand_;
    Field field_;
    Player enemy_;

	// Use this for initialization
	void Start () {
        deck_ = GetComponentInChildren<Deck>();
        hand_ = GetComponentInChildren<Hand>();
        field_ = GetComponentInChildren<Field>();

        Player[] players = GetComponentsInParent<Player>();

        for (int p_idx = 0; p_idx < players.Length; p_idx++) {
            if (players[p_idx] != this) {
                enemy_ = players[p_idx];
                break;
            }
        }
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

    public Player GetEnemy() {
        return enemy_;
    }
}
