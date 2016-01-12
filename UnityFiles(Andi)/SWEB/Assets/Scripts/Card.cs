using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour
{

    int id_;
    string card_name_;
    string card_description_;

    public CardLogic card_logic_;

    public void UseOn(Card target) {
        Debug.Log("Card " + name + " is used on card " + target.name);
        //card_logic_.UseOn(target.card_logic_);
    }
}
