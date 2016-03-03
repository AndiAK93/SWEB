using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    Deck deck_;
    Hand hand_;
    Field field_;
    Player enemy_;
    PlayerServer playerServer = null;
    PlayerClient playerClient = null;

    int health_;

    Text ects_text_;
	// Use this for initialization
	void Start () {
        deck_ = GetComponentInChildren<Deck>();
        hand_ = GetComponentInChildren<Hand>();
        field_ = GetComponentInChildren<Field>();
        ects_text_ = GetComponentInChildren<Text>();
        health_ = 20;

        if (Network.peerType == NetworkPeerType.Server) playerServer = GetComponent<PlayerServer>();
        if (Network.peerType == NetworkPeerType.Client) playerClient = GetComponent<PlayerClient>();
    }

    public void EnemyUseOn(int source_unique_id, int target_unique_id)
    {
        GetComponent<NetworkView>().RPC("SyncCardUse", RPCMode.Others, source_unique_id, target_unique_id);
    }

    [RPC]
    private void SyncCardUse(int source_unique_id, int target_unique_id)
    {
        Card source = null;
        Card target = null;
        Field playerField = GetField();
        Field enemyField = GetEnemy().GetField();

        for (int i = 0; i < hand_.cards_.Count; i++)
        {
            if (hand_.cards_[i].GetUniqueId() == source_unique_id) source = hand_.cards_[i];
        }
        for (int i = 0; i < playerField.cards_.Count; i++)
        {
            if (playerField.cards_[i].GetUniqueId() == source_unique_id) source = playerField.cards_[i];
        }

        for (int i = 0; i < playerField.cards_.Count; i++)
        {
            if (playerField.cards_[i].GetUniqueId() == target_unique_id) target = playerField.cards_[i];
        }
        for (int i = 0; i < enemyField.cards_.Count; i++)
        {
            if (enemyField.cards_[i].GetUniqueId() == target_unique_id) target = enemyField.cards_[i];
        }

        ReturnType status = source.GetCardLogic().UseOn(target.GetCardLogic());
        if (status != ReturnType.NOT_POSSIBLE)
        {
            Debug.Log("Card " + source.GetName() + " is used on card " + target.GetName());
        }
    }

    public void EnemyUseOnPlayer(int unique_id)
    {
        GetComponent<NetworkView>().RPC("SyncPlayerUse", RPCMode.Others, unique_id);
    }

    [RPC]
    private void SyncPlayerUse(int unique_id)
    {
        Card source = null;
        Field enemyField = GetEnemy().GetField();
        Field myField = GetField();
        Hand enemyHand = GetEnemy().GetHand();

        for (int i = 0; i < enemyHand.cards_.Count; i++)
        {
            if (enemyHand.cards_[i].GetUniqueId() == unique_id) source = enemyHand.cards_[i];
        }
        for (int i = 0; i < enemyField.cards_.Count; i++)
        {
            if (enemyField.cards_[i].GetUniqueId() == unique_id) source = enemyField.cards_[i];
        }
        for (int i = 0; i < myField.cards_.Count; i++)
        {
            if (myField.cards_[i].GetUniqueId() == unique_id) source = myField.cards_[i];
        }


        ReturnType status = source.GetCardLogic().UseOn(this);
        if (status != ReturnType.NOT_POSSIBLE)
        {
            Debug.Log("Card " + name + " is used on Player " + this.name);
        }
    }

    public void EnemyLeftRewardPressed(int unique_id)
    {
        GetComponent<NetworkView>().RPC("SyncLeftRewardPressed", RPCMode.Others, unique_id);
    }

    [RPC]
    private void SyncLeftRewardPressed(int unique_id)
    {
        Card source = null;
        Field playerField = GetField();

        for (int i = 0; i < playerField.cards_.Count; i++)
        {
            if (playerField.cards_[i].GetUniqueId() == unique_id) source = playerField.cards_[i];
        }

        source.GetCardLogic().LeftReward();
    }

    public void EnemyRightRewardPressed(int unique_id)
    {
        GetComponent<NetworkView>().RPC("SyncRightRewardPressed", RPCMode.Others, unique_id);
    }

    [RPC]
    private void SyncRightRewardPressed(int unique_id)
    {
        Card source = null;
        Field playerField = GetField();

        for (int i = 0; i < playerField.cards_.Count; i++)
        {
            if (playerField.cards_[i].GetUniqueId() == unique_id) source = playerField.cards_[i];
        }

        source.GetCardLogic().RightReward();
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

		if(mod > 0) {
			Game.GetGame ().playHealSound ();
		}
		else {
			Game.GetGame ().playAttackSound ();
		}

        health_ += mod;
    }

    public void RefreshVisuals() {
        ects_text_.text = health_.ToString();
    }
}
