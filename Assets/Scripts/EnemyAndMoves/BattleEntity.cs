using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BattleEntity : MonoBehaviour
{
    //Initializers
    public BattleManager battleManager;
    public GameManager gameManager;
    public AudioSource audioSource;
    public GameObject animator;

    //Stats
    public string entityName;
    public int currentHealth;
    public int maxHealth;
    public int Level;
    public int Attack;
    public int Defence;
    public float Speed;

    public int poisonDamage = 2;

    //Other
    public Unit unit;
    public bool usedMove = false;
    public bool cursed = false; //One of the Status Conditions
    public IStatusEffect statusEffect = null;
    public bool isFrozen = false;
    protected void Start()
    {
        gameManager = GameManager.instance;
        StatSet();
    }

    protected void Heal(int amount, BattleManager battleManager, Slider healthBar)
    {
        if (currentHealth + (amount * maxHealth) / 100 <= maxHealth)
        {
            battleManager.gameText.text = $"{entityName} healed {(amount * maxHealth) / 100} health";
        }
        else if (currentHealth + (amount * maxHealth) / 100 > maxHealth)
        {
            battleManager.gameText.text = $"{entityName} healed {(maxHealth - currentHealth).ToString()} health";
        }
        currentHealth = Mathf.Min(currentHealth + ((amount * maxHealth) / 100), maxHealth);
        healthBar.value = currentHealth;
    }
    public IEnumerator Cursed()
    {
        yield return new WaitForSeconds(1.5f);
        battleManager.gameText.text = $"{entityName} took {currentHealth * 0.15f} damage due to a curse";
        currentHealth = Mathf.RoundToInt(currentHealth * 0.85f);
    } //Have this controlled by status condition manager later!

    public void TakeDamage(int dmg)
    {
        int EffectiveDamage = Mathf.CeilToInt(dmg - Defence);
        if (EffectiveDamage <= 0)
        {
            EffectiveDamage = 1;
        }
        currentHealth -= EffectiveDamage;

        battleManager.gameText.text = $"{entityName} took {EffectiveDamage} damage!";
    }
    protected void StatSet()
    {
        entityName = unit.enemyName;
        maxHealth = unit.MaxHealth;
        currentHealth = maxHealth;
        Level = unit.Level;
        Attack = unit.Attack;
        Defence = unit.Defence;
        Speed = unit.Speed;
    }


    public void AddStatusEffect(UnitMoves usedMove, BattleEntity target, BattleEntity sender)
    {
        IStatusEffect effect = SwitchStatusEffects(usedMove.StatusCondition, target);
        if (effect is PoisonEffect && target.statusEffect is not PoisonEffect)
        {
            effect = new PoisonEffect(); // Create a fresh instance for this target
            target.statusEffect = effect;
        }
        else if (effect is PoisonEffect)
        {
            Debug.Log("Added more poison");
            effect.ApplyEffect(target, sender);
            return;
        }
        if(effect is FreezeEffect && target.isFrozen == true)
        {
            battleManager.gameText.text = $"{target.entityName} is already Frozen!";
            return;
        }
        target.statusEffect = effect;
        effect.ApplyEffect(target, sender);
    }

    public void UpdateStatusEffect(IStatusEffect effect, BattleEntity effected)
    {
        if(effect.Duration > 0)
        {
            effect.UpdateEffect(effected);
        }
        else
        {
            Debug.Log($"Remove {effect} from {effected}");
            SwitchStatusEffects(EnumStatusEffect.None, effected);
            effect.RemoveEffect(effected);
        }
        
    }
    public IStatusEffect SwitchStatusEffects(EnumStatusEffect effect, BattleEntity target)
    {
        switch (effect)
        {
            case EnumStatusEffect.Freeze:
                return statusEffect = new FreezeEffect();

            case EnumStatusEffect.Burn:
                return null;   //Need To Make A script

            case EnumStatusEffect.Poison:
                if (target.statusEffect is PoisonEffect)
                { 
                    return target.statusEffect; 
                }
                else
                    return statusEffect = new PoisonEffect();

            case EnumStatusEffect.Cursed:
                //Need To Make A script
                return null;

            case EnumStatusEffect.Purify:
                //Need To Make A script
                return null;

            case EnumStatusEffect.None:
                Debug.Log($"Status Effects set to null for {target}");
                return statusEffect = new NoEffect();

            default:
                return null;
        }

    }
}