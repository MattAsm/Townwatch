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

    //Other
    public Unit unit;
    public bool usedMove = false;
    public bool cursed = false; //One of the Status Conditions
    public IStatusEffect statusEffect;
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


    public void AddStatusEffect(IStatusEffect effect, BattleEntity target)
    {
        target.statusEffect = effect;
        effect.ApplyEffect(target);
    }

    public void UpdateStatusEffect(IStatusEffect effect, BattleEntity effected)
    {
        if(effect.Duration > 0)
        {
            effect.UpdateEffect(effected);
        }
          else
        {
            Debug.Log("Remove???");
            effect.RemoveEffect(effected);
        }
        
    }


    public void SwitchStatusEffects(EnumStatusEffect effect)
    {
        switch (effect)
        {
            case EnumStatusEffect.Freeze:
                statusEffect = new FreezeEffect();
                break;

            case EnumStatusEffect.Burn:
                //Need To Make A script
                break;

            case EnumStatusEffect.Poison:
                statusEffect = new PoisonEffect();
                break;

            case EnumStatusEffect.Cursed:
                //Need To Make A script
                break;

            case EnumStatusEffect.Purify:
                //Need To Make A script
                break;

            default:
                break;
        }

    }
}