public enum ReturnType { OK, FAIL, BLOCKED, NOT_POSSIBLE, NONE };



public class Effect {
    protected int value_;

    public virtual ReturnType ApplyEffect(CardLogic from, CardLogic target) {
        return ReturnType.NOT_POSSIBLE;
    }

    public virtual ReturnType ApplyEffect(CardLogic from, Player target) {
        return ReturnType.NOT_POSSIBLE;
    }

    public static Effect CreateEffect(string effect_name, int effect_value)
    {
        switch (effect_name) {
            case "lv_counter":              return new EffectModifyDuration(effect_value);
            case "swap_card":               break;
            case "destroy_lv_card":         return new EffectDestroyLVCard();
            case "get_card_from_deck":      return new EffectDrawRandomCard();
            case "modify_attack":           return new EffectModifyAttack(effect_value);
            case "modify_defense":          return new EffectModifyHealth(effect_value);
            case "modify_ects":             return new EffectModifyEcts(effect_value);
            case "modify_defense_all":      return new EffectModifyDefenseAll(effect_value);
            case "switch_attack_defense":   return new EffectSwitchAtkWithDef();
            case "encrypt":                 break;
            case "spawn_card":              return new EffectSpawnCard(effect_value);
        }
        return null;
    }

}

public class EffectSwitchAtkWithDef : Effect
{
    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        int old_atk = target.GetAttack();
        int old_def = target.GetHealth();
        target.SetAttack(old_def);
        target.SetHealth(old_atk);
        target.RefreshVisuals();
        return ReturnType.OK;
    }
}

public class EffectModifyDuration : Effect {

    public EffectModifyDuration(int value) {
        value_ = value;
    }

    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        return target.ModifyDuration(value_);
    }
}

public class EffectDestroyLVCard : Effect {
    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        target.RemoveCard();
        return ReturnType.OK;
    }
}


public class EffectModifyEcts : Effect {
    public EffectModifyEcts(int value) {
        value_ = value;
    }

    public override ReturnType ApplyEffect(CardLogic from, Player target)
    {
        target.ModifyHealth(value_);
        target.RefreshVisuals();
        return ReturnType.OK;
    }
}

public class EffectModifyAttack : Effect {
    public EffectModifyAttack(int value)
    {
        value_ = value;
    }

    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        target.ModifyAttack(value_);
        target.RefreshVisuals();
        return ReturnType.OK;
    }
}

public class EffectModifyHealth : Effect
{
    public EffectModifyHealth(int value)
    {
        value_ = value;
    }

    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        target.ModifyHealth(value_);
        target.RefreshVisuals();
        return ReturnType.OK;
    }
}

public class EffectModifyDefenseAll : Effect
{
    public EffectModifyDefenseAll(int value)
    {
        value_ = value;
    }

    public override ReturnType ApplyEffect(CardLogic from, Player target)
    {
        foreach (Card c in target.GetField().cards_) {
            c.GetCardLogic().ModifyHealth(-value_);
            c.GetCardLogic().RefreshVisuals();
        }
        return ReturnType.OK;
    }
}

public class EffectDrawRandomCard : Effect
{
    public override ReturnType ApplyEffect(CardLogic from, Player target)
    {
        card_t db_card = Game.GetGame().GetDataBank().getRandomCard();
        Card card = target.GetDeck().CreateCard(db_card);
        target.GetHand().AddCardToHand(card);
        target.GetHand().EnemyRandomCard(card.GetId());
        return ReturnType.OK;
    }
}

public class EffectSpawnCard : Effect {
    public EffectSpawnCard(int id) {
        value_ = id;
    }

    public override ReturnType ApplyEffect(CardLogic from, Player target) {
        card_t db_card = Game.GetGame().GetDataBank().getCard(value_);
        Card card = target.GetDeck().CreateCard(db_card);

        switch (db_card.cardType_) {
            case (int)CardType.Knowledge:
                target.GetField().AddCardToField(card);
                break;

            case (int)CardType.Activity:
                target.GetHand().AddCardToHand(card);
                target.GetHand().EnemyRandomCard(card.GetId());
                //card.UseOn(target);
                break;

            case (int)CardType.Lecture:
                break;
        }

        return ReturnType.OK;
    }
}