public interface IStatusEffect
{
    string Name { get; }
    EnumStatusEffect StatEff { get; }
    int Duration { get; }
    void ApplyEffect(BattleEntity target);
    void UpdateEffect(BattleEntity target);
    void RemoveEffect(BattleEntity target);
}
