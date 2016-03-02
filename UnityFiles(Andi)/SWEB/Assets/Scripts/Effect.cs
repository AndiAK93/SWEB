public enum ReturnType { OK, FAIL, BLOCKED, NOT_POSSIBLE, NONE };



public class Effect {
    protected int value_;

    public virtual ReturnType ApplyEffect(CardLogic from, CardLogic target) {
        return ReturnType.NOT_POSSIBLE;
    }

    public virtual ReturnType ApplyEffect(CardLogic from, Player target)
    {
        return ReturnType.NOT_POSSIBLE;
    }

    public static Effect CreateEffect(string effect_name, int effect_value)
    {
        switch (effect_name) {
            case "lv_counter":  return new EffectModifyDuration(effect_value);

            case "swap_card":
                break;

            case "destroy_lv_card":
                break;

            case "get_card_from_deck":
                break;

            case "modify_attack":
                break;

            case "modify_defense":
                break;

            case "modify_ects":
                break;

            case "modify_defense_all":
                break;

            case "switch_attack_defense":
                break;

            case "encrypt":
                break;

            case "spawn_card":
                break;
        }
        return null;
    }

}

public class EffectModifyDuration : Effect {

    public EffectModifyDuration(int value) {
        value_ = value;
    }

    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        return target.IncDuration(value_);
    }
}


public class EffectDestroyLVCard : Effect {
    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        target.RemoveCard();
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
        target.GetField().AddCardToField(card);
        return ReturnType.OK;
    }
}