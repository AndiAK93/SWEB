using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    // we allow only one game instance at a time
    static Game game_;

    Player[] players_;
    int round_;
    int cycle_;
    int cur_player_idx_;

    Text round_text_;
    Inspector inspector_;

    dbInterface data_base_;

    // Use this for initialization
    void Start() {
        // create data_base_ 
        data_base_ = new dbInterface();

        game_ = this;

        

        cycle_ = 2;
        round_ = 1;
        players_ = GetComponentsInChildren<Player>();

        players_[0].SetEnemy(players_[1]);
        players_[1].SetEnemy(players_[0]);

        cur_player_idx_ = 0;
        round_text_ = GetComponentsInChildren<Text>()[0];
        round_text_.text = round_.ToString();

        inspector_ = GetComponentInChildren<Inspector>();
        inspector_.HideInspector();
    }

    public static Game GetGame() {
        return game_;
    }

    public Inspector GetInspector() {
        return inspector_;
    }

    public int GetRound() {
        return round_;
    }

    public Player GetCurPlayer() {
        return players_[cur_player_idx_];
    }

    public Player GetEnemyPlayer() {
        return players_[cur_player_idx_].GetEnemy();
    }

    public void RoundEndButtonClicked() {
        if (Network.peerType == NetworkPeerType.Server && cur_player_idx_ == 1) return;
        if (Network.peerType == NetworkPeerType.Client && cur_player_idx_ == 0) return;

        Debug.Log("Round End Clicked!");
        cur_player_idx_ = (++cur_player_idx_) % 2;
        round_ = (++cycle_)/2;
        round_text_.text = round_.ToString();

        // We have to update the fieldes

        for (int p_idx = 0; p_idx < 2; p_idx++) {
            players_[p_idx].GetField().UpdateCards();
        }

        GetComponent<NetworkView>().RPC("EnemyEndButtonClicked", RPCMode.Others);
    }

    [RPC]
    void EnemyEndButtonClicked()
    {
        Debug.Log("Round End Clicked!");
        cur_player_idx_ = (++cur_player_idx_) % 2;
        round_ = (++cycle_) / 2;
        round_text_.text = round_.ToString();

        // We have to update the fieldes

        for (int p_idx = 0; p_idx < 2; p_idx++)
        {
            players_[p_idx].GetField().UpdateCards();
        }
    }
}
