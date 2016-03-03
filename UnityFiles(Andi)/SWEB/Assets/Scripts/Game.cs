using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    // we allow only one game instance at a time
    static Game game_;

    private const int initial_cards_on_hand_ = 5;

    public static int unique_card_id = 0;

    Player[] players_;
    int round_;
    int cycle_;
    public int cur_player_idx_;
    public bool canDrawCard = true;

    Text round_text_;
    Inspector inspector_;

    dbInterface data_base_;

	AudioSource audioSource;

    // Use this for initialization
    void Start() {
        game_ = this;

        data_base_ = new dbInterface();

        cycle_ = 2;
        round_ = 1;

        players_ = GetComponentsInChildren<Player>();

        players_[0].SetEnemy(players_[1]);
        players_[1].SetEnemy(players_[0]);

        if (Network.peerType == NetworkPeerType.Server)
        {
            players_[0].GetDeck().CreateRandomDeck();
            players_[1].GetDeck().CreateRandomDeck();

            for (int i = 0; i < initial_cards_on_hand_; i++)
            {
                players_[0].GetDeck().InitialDrawCardFromDeck();
                players_[1].GetDeck().InitialDrawCardFromDeck();
            }
            players_[0].GetDeck().InitialDrawCardFromDeck();
        }       

        cur_player_idx_ = 0;

        round_text_ = GetComponentsInChildren<Text>()[0];
        round_text_.text = round_.ToString();

        inspector_ = GetComponentInChildren<Inspector>();
        inspector_.HideInspector();
		audioSource = gameObject.AddComponent<AudioSource>();
    }

    public dbInterface GetDataBank() {
        return data_base_;
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
        if (!IsMyTurn()) return;

        Debug.Log("Round End Clicked!");

        round_ = (++cycle_)/2;
        round_text_.text = round_.ToString();

        // We have to update the fieldes
        //for (int p_idx = 0; p_idx < 2; p_idx++) {
        players_[cur_player_idx_].GetField().UpdateCards();
        cur_player_idx_ = (++cur_player_idx_) % 2;
        //}

        GetComponent<NetworkView>().RPC("EnemyEndButtonClicked", RPCMode.Others);
        canDrawCard = true;
		playRoundEndSound ();
    }

	public void playRoundEndSound()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/effect1") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.8f);
	}

	public void playAttackSound()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/attack2") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	} 

	public void playDropSound(){
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/click3") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	}

	public void playDestroySound()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/kill2") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	}

	public void playDrawSound()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/draw1") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	}

	public void playHealSound()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = Resources.Load ("sound/heal") as AudioClip;
		audioSource.PlayOneShot (audioSource.clip, 0.4f);
	}

    [RPC]
    void EnemyEndButtonClicked()
    {
        Debug.Log("Round End Clicked!");

        round_ = (++cycle_) / 2;
        round_text_.text = round_.ToString();


        players_[cur_player_idx_].GetField().UpdateCards();
        cur_player_idx_ = (++cur_player_idx_) % 2;
        canDrawCard = true;
        players_[cur_player_idx_].GetDeck().DrawCardFromDeck();

		playRoundEndSound ();
    }

    public bool IsMyTurn()
    {
        if (Network.peerType == NetworkPeerType.Client && cur_player_idx_ == 0)
            return true;
        else if (Network.peerType == NetworkPeerType.Server && cur_player_idx_ == 1)
            return true;
        return false;
    }
}
