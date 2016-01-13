using System.Collections;

public enum ReturnType { OK, FAIL, BLOCKED, NOT_POSSIBLE, NONE };

public class Effect
{
    public virtual ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        return ReturnType.NOT_POSSIBLE;
    }

    /*public virtual Status ApplyEffect(Card from, Player target)
    {
        return Status.NOT_POSSIBLE;
    }*/
}

public class EffectIncHealth : Effect
{
    int inc_ = 3;
    public override ReturnType ApplyEffect(CardLogic from, CardLogic target)
    {
        return target.IncHealth(inc_);
    }

    /*public override Status ApplyEffect(CardBase from, Player target)
    {
        //return target.IncHealth(inc_);
        return Status.NOT_POSSIBLE;
    }*/
}