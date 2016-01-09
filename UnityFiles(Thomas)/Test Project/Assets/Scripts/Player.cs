using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public GameObject go_deck_;
	Deck deck_;

	public List<GameObject> go_hand_;
	public List<GameObject> go_board_;

	List<CardHolder> hand_;
	List<CardHolder> board_;

	int hand_counter_;
	int board_counter_;

	int life_;

	static class PlayerConstants
	{
		public const int StartLife = 10;
		public const int TimePerRound = 30;
		public const int CardsOnHand = 3;
		public const int CardsOnBoard = 3;
	}

	// Use this for initialization
	void Start()
	{
		hand_counter_ = 0;
		board_counter_ = 0;

		life_ = PlayerConstants.StartLife;
		deck_ = go_deck_.GetComponent<Deck>();

		hand_ = new List<CardHolder>();
		board_ = new List<CardHolder>();

		for(int counter = 0 ; counter < PlayerConstants.CardsOnHand ; counter++)
		{
			hand_.Add(go_hand_[counter].GetComponent<CardHolder>());
		}

		for(int counter = 0 ; counter < PlayerConstants.CardsOnBoard ; counter++)
		{
			board_.Add(go_board_[counter].GetComponent<CardHolder>());
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
	
	// nimmt eine karte aus seinem deck und fügt sie der hand hinzu
	public void DrawCards(int num_of_cards)
	{
		Card current_card;
		CardHolder current_hand_holder;

		for(int counter = 0 ; counter < num_of_cards && hand_counter_ < PlayerConstants.CardsOnHand ; counter++)
		{
			current_card = deck_.drawCard();

			if(current_card != null)
			{
				hand_[hand_counter_].placeCard(current_card);
				hand_[hand_counter_].showCard();
				hand_counter_++;
			}
		}
	}

	public void playCard()
	{
		Card current_card;

		if(hand_counter_ > 0)
		{
			current_card = hand_[hand_counter_ - 1].popCard();
			hand_counter_--;
			
			//CardHolder current_board_holder = go_board_[board_counter_].GetComponent<CardHolder>();
			
			board_[board_counter_].placeCard(current_card);
			board_counter_++;
		}
		else
		{
			Debug.Log("Player.cs: no more cards on hand.");
		}
	}
}
