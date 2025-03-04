using UnityEngine;

public class PoisonEffect : IStatusEffect
{
    public string Name => "Poison";
    public EnumStatusEffect StatEff => EnumStatusEffect.Poison;
    public int Duration { get; private set; }
    
    public  void ApplyEffect(BattleEntity target, BattleEntity sender)
    {
        Duration += sender.poisonDamage;
        target.battleManager.gameText.text = $"{target.entityName} is poisoned for {Duration}!";
        Debug.Log($"Poison Applied to {target}");
    }

    public void UpdateEffect(BattleEntity target)
    {
        if (Duration > 0)
        {
           target.battleManager.gameText.text = $"{target.entityName} took {Duration} poison damage!";
            target.currentHealth -= Duration; //Duration = poisons damage. Similar to Slay The Spire
            Duration--;
            Debug.Log($"{target} Poisoned for {Duration} more turns");
        }
    }
    public void RemoveEffect(BattleEntity target)
    {
        target.statusEffect = null;
        Debug.Log("Poison Ended");
    }
}
