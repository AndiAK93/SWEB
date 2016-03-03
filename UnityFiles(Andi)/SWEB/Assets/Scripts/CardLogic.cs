using System;
using UnityEngine;
using UnityEngine.UI;

public enum CardType { Knowledge = 3, Activity = 1, Lecture = 2 };

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
    public virtual ReturnType ModifyHealth(int mod) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType ModifyAttack(int mod) { return ReturnType.NOT_POSSIBLE; }

    public virtual int GetAttack() { return 0; }
    public virtual int GetHealth() { return 0; }

    // for lecture
    public virtual ReturnType ModifyDuration(int mod) { return ReturnType.NOT_POSSIBLE; }

    // this function updates the card, wenn sie gekilled wurde bzw fertig ist etc
    public virtual void Update() { }
    public void RemoveCard() {
        card_.Kill();
    }

    public abstract void RefreshVisuals();

    public virtual void LeftReward() {  }
    public virtual void RightReward() { }
}


public class CardKnowledge : CardLogic {
    int attack_ = 0;
    int health_ = 0;

    Text name_text_;
    Text health_text_;
    Text attack_text_;

    Image portrait_image_;

    Effect effect_ = null;

    public CardKnowledge(Card card, int attack, int health, Effect effect) {
        card_ = card;
        type_ = CardType.Knowledge;
        attack_ = attack;
        health_ = health;

        name_text_ = card.GetComponentsInChildren<Text>()[0];
        health_text_ = card.GetComponentsInChildren<Text>()[1];
        attack_text_ = card.GetComponentsInChildren<Text>()[2];

        portrait_image_ = card.GetComponentsInChildren<Image>()[1];

        effect_ = effect;

        attack_text_.text = attack.ToString();
        health_text_.text = health.ToString();
    }

    public override void RefreshVisuals()
    {
        name_text_.text = card_.card_name_;
        attack_text_.text = attack_.ToString();
        health_text_.text = health_.ToString();
        portrait_image_.overrideSprite = Resources.Load<Sprite>(card_.image_left_);
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

    public override ReturnType ModifyHealth(int mod) {
        health_ += mod;
        health_text_.text = health_.ToString();
        return ReturnType.OK;
    }

    public override ReturnType ModifyAttack(int mod) {
        attack_ += mod;
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
            this.ModifyHealth(-target.GetAttack());
            this.Update();
            return ReturnType.OK;
        } else if (status_this == ReturnType.BLOCKED) {
            target.ModifyHealth(-this.GetAttack());
            target.Update();
            return ReturnType.OK;
        }

        // do the base calcs
        target.ModifyHealth(-this.GetAttack());
        this.ModifyHealth(-target.GetAttack());

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
    int cost_;


    Text name_text_;
    Text cost_text_;
    Image portrait_image_;

    public CardActivity(Card card, int cost, Effect effect) {
        card_ = card;
        type_ = CardType.Activity;
        effect_ = effect;
        cost_ = cost;

        name_text_ = card.GetComponentsInChildren<Text>()[0];
        cost_text_ = card.GetComponentsInChildren<Text>()[1];

        portrait_image_ = card.GetComponentsInChildren<Image>()[1];
    }

    public override void RefreshVisuals()
    {
        portrait_image_.overrideSprite = Resources.Load<Sprite>(card_.image_left_);
        name_text_.text = card_.card_name_;
        cost_text_.text = cost_.ToString();
    }

    public override Effect GetEffect() {
        return effect_;
    }

    public override ReturnType PlayCard() {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(Player player)
    {
        Effect effect_this = GetEffect();
        if (effect_this != null)
        {
            if (effect_this.ApplyEffect(this, player) == ReturnType.OK)
            {
                this.RemoveCard();
                this.card_.GetPlayer().ModifyHealth(cost_);
                this.card_.GetPlayer().RefreshVisuals();
                return ReturnType.OK;
            }
        }
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(CardLogic target) {
        Effect effect_this = GetEffect();

        // wenn die angreifende karte einen effekt hat wende diesen auf das target an
        if (effect_this != null) {
            if (effect_this.ApplyEffect(this, target) == ReturnType.OK) {
                this.RemoveCard();

                this.card_.GetPlayer().ModifyHealth(cost_);
                this.card_.GetPlayer().RefreshVisuals();
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

    Effect effect_left_;
    Effect effect_right_;

    Effect[] effect_additional_;

    Text name_text_;
    Text duration_text_;
    Text min_round_text_;


    Button portrait_left_btn_;
    Button portrait_right_btn_;

    public CardLecture(Card card, int min_round, int duration, Effect effect_left, Effect effect_right) {
        card_ = card;
        type_ = CardType.Lecture;

        min_round_ = min_round;
        duration_ = duration;
        played_in_round_ = -1;

        effect_left_ = effect_left;
        effect_right_ = effect_right;

        name_text_ = card.GetComponentsInChildren<Text>()[0];
        duration_text_ = card.GetComponentsInChildren<Text>()[1];
        min_round_text_ = card.GetComponentsInChildren<Text>()[2];

        portrait_left_btn_ = card.GetComponentsInChildren<Button>()[0];
        portrait_right_btn_ = card.GetComponentsInChildren<Button>()[1];

        portrait_left_btn_.interactable = false;
        portrait_right_btn_.interactable = false;
    }

    public override void RefreshVisuals()
    {
        name_text_.text = card_.card_name_;
        duration_text_.text = duration_.ToString();
        min_round_text_.text = min_round_.ToString();

        portrait_left_btn_.image.overrideSprite = Resources.Load<Sprite>(card_.image_left_);
        portrait_right_btn_.image.overrideSprite = Resources.Load<Sprite>(card_.image_right_);
    }

    public override void Update() {
        if (played_in_round_ != -1 && Game.GetGame().GetRound() >= (played_in_round_ + duration_)) {
            portrait_left_btn_.interactable = true;
            portrait_right_btn_.interactable = true;
        }
    }

    public override void LeftReward()
    {
        effect_left_.ApplyEffect(this, card_.GetPlayer());
        this.RemoveCard();
    }

    public override void RightReward()
    {
        effect_right_.ApplyEffect(this, card_.GetPlayer());
        this.RemoveCard();
    }

    public override ReturnType ModifyDuration(int mod) {
        duration_ += mod;
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