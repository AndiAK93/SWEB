using UnityEngine;
using System.Collections;

public class CardHolder : MonoBehaviour
{
	SpriteRenderer sprite_renderer_;
	public Sprite empty_holder_;
	Card holding_card_;

	// Use this for initialization
	void Start()
	{
		sprite_renderer_ = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public void placeCard(Card new_card)
	{
		holding_card_ = new_card;
		showCard();
	}

	public Card popCard()
	{
		showEmpty();
		return holding_card_;
	}

	public void showEmpty()
	{
		sprite_renderer_.sprite = empty_holder_;
	}

	public void showCard()
	{
		sprite_renderer_.sprite = holding_card_.getCardFace();
	}
	
	public void hideCard()
	{
		sprite_renderer_.sprite = holding_card_.card_back_;
	}
}
