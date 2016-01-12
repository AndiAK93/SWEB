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
        for (int idx = 0; idx < players.Length; idx++) {
            if (players[idx] != this) {
                enemy_ = players[idx];
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
