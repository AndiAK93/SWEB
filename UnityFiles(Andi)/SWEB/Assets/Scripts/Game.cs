using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    int round_;
    Player[] players_;
    Player cur_player_;

	// Use this for initialization
	void Start () {
        round_ = 0;
        players_ = GetComponents<Player>();
        cur_player_ = players_[0];



    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
