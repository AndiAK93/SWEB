using UnityEngine;
public enum CardType { Knowledge, Activity, Lecture };

public abstract class CardLogic
{
    public CardType type_;     //for effect

    protected Card card_;

    public virtual Effect GetEffect()
    {
        return null;
    }

    // die karte wird auf das target gespielt
    public abstract ReturnType UseOn(CardLogic target);
    public abstract ReturnType PlayCard();

    // for knowledge
    public virtual ReturnType IncHealth(int inc) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType DecHealth(int dec) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType IncAttack(int inc) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType DecAttack(int dec) { return ReturnType.NOT_POSSIBLE; }

    // for lecture
    public virtual ReturnType IncDuration(int inc) { return ReturnType.NOT_POSSIBLE; }
    public virtual ReturnType DecDuration(int dec) { return ReturnType.NOT_POSSIBLE; }

    public virtual int GetAttack() { return 0; }
    public virtual int GetHealth() { return 0; }

    // this function updates the card, wenn sie gekilled wurde bzw fertig ist etc
    public virtual void Update() { }
    public void RemoveCard() {
        card_.Kill();
    }
}


public class CardKnowledge : CardLogic
{
    int attack_ = 0;
    int health_ = 0;
    Effect effect_ = null;

    public CardKnowledge(Card card, int attack, int health, Effect effect) {
        card_ = card;
        type_ = CardType.Knowledge;
        attack_ = attack;
        health_ = health;
        effect_ = effect;
    }

    public override Effect GetEffect()
    {
        return effect_;
    }

    public override void Update()
    {
        if (health_ <= 0)
        {
            RemoveCard();
        }
    }

    public override int GetAttack() { return attack_; }
    public override int GetHealth() { return health_; }
    public override ReturnType IncHealth(int inc)
    {
        health_ += inc;
        return ReturnType.OK;
    }

    public override ReturnType DecHealth(int dec)
    {
        health_ -= dec;
        return ReturnType.OK;
    }

    public override ReturnType IncAttack(int inc)
    {
        attack_ += inc;
        return ReturnType.OK;
    }

    public override ReturnType DecAttack(int dec)
    {
        attack_ -= dec;
        return ReturnType.OK;
    }

    public override ReturnType PlayCard()
    {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(CardLogic target)
    {
        if (target.type_ != CardType.Knowledge)
        {
            return ReturnType.NOT_POSSIBLE;
        }

        Effect effect_target = target.GetEffect();
        Effect effect_this = GetEffect();

        ReturnType status_target = ReturnType.NONE;
        ReturnType status_this = ReturnType.NONE;

        // wenn die karte die ich angreife einen effect hat wende diesen auf die angreifende karte an
        if (effect_target != null)
        {
            status_target = effect_target.ApplyEffect(target, this);
        }

        // wenn die angreifende karte einen effekt hat wende diesen auf das target an
        if (effect_this != null)
        {
            status_this = effect_this.ApplyEffect(this, target);
        }

        // wenn beide karten ein schild haben was einen angriff komplet abwehrt
        if (status_target == ReturnType.BLOCKED && status_this == ReturnType.BLOCKED)
        {
            return ReturnType.OK;
        }
        // wenn nur der gegner ein schild hat
        else if (status_target == ReturnType.BLOCKED)
        {
            this.DecHealth(target.GetAttack());
            this.Update();
            return ReturnType.OK;
        }
        else if (status_this == ReturnType.BLOCKED)
        {
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
}



public class CardActivity : CardLogic
{
    Effect effect_;

    public CardActivity()
    {
        type_ = CardType.Activity;
    }

    public override Effect GetEffect()
    {
        return effect_;
    }

    public override ReturnType PlayCard()
    {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType UseOn(CardLogic target)
    {
        Effect effect_this = GetEffect();

        // wenn die angreifende karte einen effekt hat wende diesen auf das target an
        if (effect_this != null)
        {
            effect_this.ApplyEffect(this, target);
        }

        this.RemoveCard();

        target.Update();
        return ReturnType.OK;
    }
}

public class CardLecture : CardLogic
{
    int min_round_;
    int duration_;
    int played_in_round_;

    Effect effect_base_;
    Effect[] effect_additional_;

    public CardLecture()
    {
        type_ = CardType.Lecture;
    }

    public override void Update()
    {
        /* if (GetRound() >= (played_in_round_ + duration_))
         {
             //effect_base_.ApplyEffect(this, player_);
         }*/
    }

    public override ReturnType IncDuration(int inc)
    {
        duration_ += inc;
        return ReturnType.OK;
    }

    public override ReturnType DecDuration(int dec)
    {
        duration_ -= dec;
        return ReturnType.OK;
    }

    public override ReturnType UseOn(CardLogic target)
    {
        return ReturnType.NOT_POSSIBLE;
    }

    public override ReturnType PlayCard()
    {
        //played_in_round_ = GetRound();
        this.Update();
        return ReturnType.OK;
    }
}