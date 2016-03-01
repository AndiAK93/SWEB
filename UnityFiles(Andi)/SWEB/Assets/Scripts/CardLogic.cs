using System;
using UnityEngine;
using UnityEngine.UI;

public enum CardType { Knowledge, Activity, Lecture };

public abstract class CardLogic {
    public CardType type_;     //for effect

    protected Card card_;

    public virtual Effect GetEffect() {
        return null;
    }

    // die karte wird auf das target gespielt
    public abstract ReturnType UseOn(CardLogic target);
    public abstract ReturnType UseOn(Player player);
  
    public abstract ReturnType PlayCard();
    public abstract bool CanBeUsedFromHand();
    public abstract bool CanBePutOnField();

    // for knowledge
    public virtual ReturnType IncHealth(int inc) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType DecHealth(int dec) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType IncAttack(int inc) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType DecAttack(int dec) { return ReturnType.NOT_POSSIBLE; }

    public virtual int GetAttack() { return 0; }
    public virtual int GetHealth() { return 0; }

    // for lecture
    public virtual ReturnType IncDuration(int inc) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType DecDuration(int dec) { return ReturnType.NOT_POSSIBLE; }

    // this function updates the card, wenn sie gekilled wurde bzw fertig ist etc
    public virtual void Update() { }
    public void RemoveCard() {
        card_.Kill();
    }
}


public class CardKnowledge : CardLogic {
    int attack_ = 0;
    int health_ = 0;

    Text attack_text_;
    Text health_text_;
    
    Effect effect_ = null;

    public CardKnowledge(Card card, int attack, int health, Effect effect) {
        card_ = card;
        type_ = CardType.Knowledge;
        attack_ = attack;
        health_ = health;

        attack_text_ = card.GetComponentsInChildren<Text>()[Card.IDX_ATTACK_TEXT];
        health_text_ = card.GetComponentsInChildren<Text>()[Card.IDX_HEALTH_TEXT];

        effect_ = effect;

        attack_text_.text = attack.ToString();
        health_text_.text = health.ToString();
    }

    public override Effect GetEffect() {
        return effect_;
    }

    public override void Update() {
        if (health_ <= 0) {
            RemoveCard();
        }
    }

    public override int GetAttack() { return attack_; }
    public override int GetHealth() { return health_; }

    public override ReturnType IncHealth(int inc) {
        health_ += inc;
        health_text_.text = health_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType DecHealth(int dec) {
        health_ -= dec;
        health_text_.text = health_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType IncAttack(int inc) {
        attack_ += inc;
        attack_text_.text = attack_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType DecAttack(int dec) {
        attack_ -= dec;
        attack_text_.text = attack_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType PlayCard() {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(Player player) {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(CardLogic target) {
        if (target.type_ != CardType.Knowledge) {
            return ReturnType.NOT_POSSIBLE;
        }

        Effect effect_target = target.GetEffect();
        Effect effect_this = GetEffect();

        ReturnType status_target = ReturnType.NONE;
        ReturnType status_this = ReturnType.NONE;

        // wenn die karte die ich angreife einen effect hat wende diesen auf die angreifende karte an
        if (effect_target != null) {
            status_target = effect_target.ApplyEffect(target, this);
        }

        // wenn die angreifende karte einen effekt hat wende diesen auf das target an
        if (effect_this != null) {
            status_this = effect_this.ApplyEffect(this, target);
        }

        // wenn beide karten ein schild haben was einen angriff komplet abwehrt
        if (status_target == ReturnType.BLOCKED && status_this == ReturnType.BLOCKED) {
            return ReturnType.OK;
        }
        // wenn nur der gegner ein schild hat
        else if (status_target == ReturnType.BLOCKED) {
            this.DecHealth(target.GetAttack());
            this.Update();
            return ReturnType.OK;
        } else if (status_this == ReturnType.BLOCKED) {
            target.DecHealth(this.GetAttack());
            target.Update();
            return ReturnType.OK;
        }

        // do the base calcs
        target.DecHealth(this.GetAttack());
        this.DecHealth(target.GetAttack());

        target.Update();
        this.Update();
        return ReturnType.OK;
    }

    public override bool CanBeUsedFromHand() {
        return false;
    }

    public override bool CanBePutOnField() {
        return true;
    }
}

public class CardActivity : CardLogic {
    Effect effect_;

    public CardActivity(Card card, Effect effect) {
        card_ = card;
        type_ = CardType.Activity;
        effect_ = effect;
    }

    public override Effect GetEffect() {
        return effect_;
    }

    public override ReturnType PlayCard() {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(Player player) {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(CardLogic target) {
        Effect effect_this = GetEffect();

        // wenn die angreifende karte einen effekt hat wende diesen auf das target an
        if (effect_this != null) {
            if (effect_this.ApplyEffect(this, target) == ReturnType.OK) {
                this.RemoveCard();

                target.Update();
                return ReturnType.OK;
            }  
        } 
        return ReturnType.NOT_POSSIBLE;
    }

    public override bool CanBeUsedFromHand() {
        return true;
    }

    public override bool CanBePutOnField() {
        return false;
    }
}

public class CardLecture : CardLogic {
    int min_round_;
    int duration_;
    int played_in_round_;

    Effect effect_base_;
    Effect[] effect_additional_;

    Text min_round_text_;
    Text duration_text_;

    public CardLecture(Card card, int min_round, int duration, Effect effect_base, Effect[] effect_additional) {
        card_ = card;
        type_ = CardType.Lecture;

        min_round_ = min_round;
        duration_ = duration;
        played_in_round_ = -1;

        effect_base_ = effect_base;
        effect_additional_ = effect_additional;

        min_round_text_ = card.GetComponentsInChildren<Text>()[Card.IDX_MIN_ROUND_TEXT];
        duration_text_ = card.GetComponentsInChildren<Text>()[Card.IDX_DURATION_TEXT];

        min_round_text_.text = min_round_.ToString();
        duration_text_.text = duration_.ToString();
    }

    public override void Update() {
        if (played_in_round_ != -1 && Game.GetGame().GetRound() >= (played_in_round_ + duration_)) {
            effect_base_.ApplyEffect(this, card_.GetPlayer());
            this.RemoveCard();
        }
    }

    public override ReturnType IncDuration(int inc) {
        duration_ += inc;
        duration_text_.text = duration_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType DecDuration(int dec) {
        duration_ -= dec;
        duration_text_.text = duration_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType UseOn(Player player) {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(CardLogic target) {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType PlayCard() {
        played_in_round_ = Game.GetGame().GetRound();
        this.Update();
        return ReturnType.OK;
    }

    public override bool CanBeUsedFromHand() {
        return false;
    }

    public override bool CanBePutOnField() {
        return true;
    }
}