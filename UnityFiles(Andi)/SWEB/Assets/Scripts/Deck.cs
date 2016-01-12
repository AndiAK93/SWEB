using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Deck : MonoBehaviour {
    public GameObject card_for_init_;

    List<GameObject> cards_;

    class Constatns {
        public static int CardsPerDeck = 2;
    }

    // Use this for initialization
    void Start () {
        cards_ = new List<GameObject>();
        // create deck
        for (int idx = 0; idx < Constatns.CardsPerDeck; idx++) {
            GameObject new_card = Instantiate(card_for_init_);
            new_card.transform.SetParent(this.transform);
            new_card.transform.position = this.transform.position;
            new_card.name = "Card " + idx.ToString();
            new_card.GetComponent<Image>().enabled = false;
            cards_.Add(new_card);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseDown() {
        if (cards_.Count > 0)
        {
            GameObject card = cards_[0];
            cards_.RemoveAt(0);
            card.GetComponent<Image>().enabled = true;
            card.transform.SetParent(this.transform.parent.GetChild(1).transform);
        }
    }
}
