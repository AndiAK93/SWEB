using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

    static class Constants
    {
        public const int StartLife = 10;
        public const int TimePerRound = 30;
        public const int CardsPerDeck = 10;
        public const int CardsOnHand = 5;
    }

    class Effect {

    }

    class Reward {

    }

    class CardReward : Reward{
        Card cardReward;
    }

    class EctsReward : Reward {
        int ectsReward;
    }

    class Card
    {
        string name;
        string description;

        // bilder
    }

    class AKCard : Card
    {
        int cost;
        Effect effect;
    }

    class LVCard : Card
    {
        int duration;
        int minRound;
        int playedInRound;

        Reward fixReward;
        Reward selectiveReward;

        int selectedReward;
    }

    class KLCard : Card
    {
        int attack;
        int life;

        Effect effect;
    }

    class Deck {
        
    }

    class Player {
        Card[] Deck;
        Card[] Hand;
        Card[] Board;
        int Life;
       
        public Player() {
            Life = Constants.StartLife;
            Deck = CreateRandDeck();
            Hand = Draw(Deck);
        }
        public static Card[] CreateRandDeck() {
            return new Card[Constants.CardsPerDeck];
        }
        public static Card[] Draw(Card[] Deck) {
            return Deck;
        }

        // nimmt eine karte aus seinem deck und fügt sie der hand hinzu
        public void DrawSingleCard() {

        }
    }

   
    Player[] Players;
    
    int currentRound;

    class Timer {
        public void Start() { }
    }

    Timer roundTimer;  

    // Use this for initialization
    void Start () {
        // LoadCards Generate Cards;
        // ShowMenu
        Players = new Player[2];
        Players[0] = new Player();
        Players[1] = new Player();

        // PrePlay

        Debug.Log("Start fin");
        currentRound = 0;

        
    }
	



	// Update is called once per frame
	void Update () {
	
	}

    void StartTurn() {
        Player curPlayer = Players[currentRound % 2];


        curPlayer.DrawSingleCard();

        roundTimer.Start();  
    }

    public void EndTurn() {
        Debug.Log("End Turn");
    }
}
    