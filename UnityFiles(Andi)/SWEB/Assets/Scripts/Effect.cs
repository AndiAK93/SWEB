public enum ReturnType { OK, FAIL, BLOCKED, NOT_POSSIBLE, NONE };

public class Effect {
    public virtual ReturnType ApplyEffect(CardLogic from, CardLogic target) {
        return ReturnType.NOT_POSSIBLE;
    }

    public virtual ReturnType ApplyEffect(CardLogic from, Player target) {
        return ReturnType.NOT_POSSIBLE;
    }
}

public class EffectIncHealth : Effect {
    int inc_ = 3;
    public override ReturnType ApplyEffect(CardLogic from, CardLogic target) {
        return target.IncHealth(inc_);
    }

    /*public override Status ApplyEffect(CardBase from, Player target)
    {
        //return target.IncHealth(inc_);
        return Status.NOT_POSSIBLE;
    }*/
}

public class EffectSpawnCard : Effect {
    public override ReturnType ApplyEffect(CardLogic from, Player target) {
        Card new_card = target.GetDeck().CreateCard(0);
        target.GetField().AddCardToField(new_card);
        return ReturnType.OK;
    }
}