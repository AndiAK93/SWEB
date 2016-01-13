using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Text health_;
    public Text attack_;
    int id_;
    string card_name_;
    string card_description_;

    public CardLogic card_logic_;

    void Start()
    {
        card_logic_ = new CardKnowledge(this, 5, 6, new EffectIncHealth());

        health_ = GetComponentsInChildren<Text>()[0];
        attack_ = GetComponentsInChildren<Text>()[1];

        health_.text = card_logic_.GetHealth().ToString();
        attack_.text = card_logic_.GetAttack().ToString();
    }

    void Update() {
        health_.text = card_logic_.GetHealth().ToString();
        attack_.text = card_logic_.GetAttack().ToString();
       
    }

    public void UseOn(Card target) {
        Debug.Log("Card " + name + " is used on card " + target.name);
        ReturnType status = card_logic_.UseOn(target.card_logic_);
    }

    public void Kill() {
        Destroy(this.gameObject);
    }

}
