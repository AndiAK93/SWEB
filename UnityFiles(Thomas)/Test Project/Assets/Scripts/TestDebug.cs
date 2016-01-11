using UnityEngine;
using System.Collections;

public class TestDebug : MonoBehaviour
{
	public GameObject go_player_1_;
	//public GameObject go_player_2_;

	Player player_1_;
	//Player player_2_;

	// Use this for initialization
	void Start()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake()
	{
		player_1_ = go_player_1_.GetComponent<Player>();
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect(0, 0, 100, 28), "Draw Card!") && player_1_)
		{
			player_1_.DrawCards(1);
		}

		if (GUI.Button (new Rect(0, 50, 100, 28), "Play Card!") && player_1_)
		{
			player_1_.playCard();
		}
	}
}
