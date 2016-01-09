using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
	SpriteRenderer sprite_renderer_;
	List<int> deck_id_list_;
	public Sprite deck_face_;
	public GameObject go_card_;
	int draw_counter_;

	static class DeckConstants
	{
		public const int CardsPerDeck = 3;
	}

	// Use this for initialization
	void Start()
	{
		draw_counter_ = DeckConstants.CardsPerDeck;

		sprite_renderer_ = GetComponent<SpriteRenderer>();
		sprite_renderer_.sprite = deck_face_;
		
		deck_id_list_ = new List<int>();
		//todo: randomize
		for(int counter = 0 ; counter < DeckConstants.CardsPerDeck ; counter++)
		{
			deck_id_list_.Add(counter + 1);
		}
	}

	void Awake()
	{

	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	
	// === Methods ===
	
	public Card drawCard()
	{
		Card current_card_;

		if(draw_counter_ > 0)
		{
			current_card_ = go_card_.GetComponent<Card>();
			current_card_.applyProperties(deck_id_list_[0]);
			deck_id_list_.RemoveAt(0);
			draw_counter_--;
			return current_card_;
		}
		else
		{
			Debug.Log("Deck.cs: no more cards in deck.");
			return null;
		}
	}
}
