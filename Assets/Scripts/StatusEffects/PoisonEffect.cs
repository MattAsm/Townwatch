using UnityEngine;

public class PoisonEffect : IStatusEffect
{
    public string Name => "Freeze";
    public EnumStatusEffect StatEff => EnumStatusEffect.Poison;
    public int Duration { get; private set; }

    private int DamagePerTurn; //Set this to the damage per turn, depending on what kind of poison I decide on

    /// <summary>
    /// Keeping Code Blank for now. Need to decide on how I want the effect to work. 
    /// Likely make it like "Slay The Spire" as I really like their idea for a poison effect
    /// </summary>
    
    public void ApplyEffect(BattleEntity target)
    {
        Debug.Log("Poison Applied");
    }
    public void UpdateEffect(BattleEntity target)
    {
        Debug.Log("Poison Updated");
    }
    public void RemoveEffect(BattleEntity target)
    {
        Debug.Log("Poison Ended");
    }
}
