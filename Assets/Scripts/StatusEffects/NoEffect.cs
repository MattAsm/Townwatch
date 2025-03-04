using UnityEngine;

public class NoEffect : IStatusEffect
{
    public string Name => "None";
    public EnumStatusEffect StatEff => EnumStatusEffect.None;
    public int Duration { get; private set; }

    public void ApplyEffect(BattleEntity target, BattleEntity sender)
    {
        return;
    }
    public void UpdateEffect(BattleEntity target)
    {
        return;
    }
    public void RemoveEffect(BattleEntity target)
    {
        return;
    }
}

