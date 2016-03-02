using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    Deck deck_;
    Hand hand_;
    Field field_;
    Player enemy_;
    PlayerServer playerServer = null;
    PlayerClient playerClient = null;

    int health_;


	// Use this for initialization
	void Start () {
        deck_ = GetComponentInChildren<Deck>();
        hand_ = GetComponentInChildren<Hand>();
        field_ = GetComponentInChildren<Field>();

        if (Network.peerType == NetworkPeerType.Server) playerServer = GetComponent<PlayerServer>();
        if (Network.peerType == NetworkPeerType.Client) playerClient = GetComponent<PlayerClient>();
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

    public PlayerServer GetPlayerServer()
    {
        return playerServer;
    }

    public PlayerClient GetPlayerClient()
    {
        return playerClient;
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
