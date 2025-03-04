using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FreezeEffect : IStatusEffect
{
    public string Name => "Freeze";
    public EnumStatusEffect StatEff => EnumStatusEffect.Freeze;
    public int Duration { get; private set; }
       
    public void ApplyEffect(BattleEntity target, BattleEntity sender)
    {
        this.Duration = 3;
        target.isFrozen = true;
        target.battleManager.gameText.text = $"{target.entityName} is frozen for {Duration - 1} turns!";
        Debug.Log($"Freeze Applied for {this.Duration} turns");
    }
    public void UpdateEffect(BattleEntity target)
    {
        if (this.Duration > 0)
        {
            this.Duration--;
            Debug.Log($"Still Frozen for {this.Duration} more turns");
        }
    }
    public void RemoveEffect(BattleEntity target)
    {
        target.isFrozen = false;
        target.statusEffect = new NoEffect();
    }
}

