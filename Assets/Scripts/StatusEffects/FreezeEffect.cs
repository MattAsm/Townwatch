using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FreezeEffect : IStatusEffect
{
    public string Name => "Freeze";
    public EnumStatusEffect StatEff => EnumStatusEffect.Freeze;
    public int Duration { get; private set; }
       
    public void ApplyEffect(BattleEntity target)
    {
        Duration = Random.Range(2, 4);
        target.isFrozen = true;
        Debug.Log($"Freeze Applied");
    }
    public void UpdateEffect(BattleEntity target)
    {
        if (Duration > 0)
        {
            Duration--;
            Debug.Log($"Still Frozen for {Duration} more turns");
        }
    }
    public void RemoveEffect(BattleEntity target)
    {
        target.isFrozen = false;
        target.statusEffect = null;
    }
}

