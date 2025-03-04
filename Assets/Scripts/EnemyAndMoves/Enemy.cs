using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : BattleEntity
{
    private bool isDead = false;

    public Slider healthBar;

    public GameObject backAnimation;
    public CameraShake cameraShake;

    public int hitCount = 0;
    private bool doneHit = false;
   new void Start()
    {
        base.Start();
        battleManager = BattleManager.Instance;
        healthBar = GameObject.FindGameObjectWithTag("enemyHealth").GetComponent<Slider>();
        animator = GameObject.FindGameObjectWithTag("enemyAnimator");
        backAnimation = GameObject.FindGameObjectWithTag("healBackAnimator");
        cameraShake = FindFirstObjectByType<CameraShake>();

        poisonDamage = unit.poisonDamage;

        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    private void Update()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    public new void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if (currentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(Die());
        }

        healthBar.value = currentHealth;
    }

    public new void Heal(int amount, BattleManager battleManager, Slider healthBar)
    {
        base.Heal(amount, battleManager, healthBar);
    }//Polymorphismed

    public IEnumerator TakeTurn()
    {
        audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();
        if (!isDead)
        {
            battleManager.gameText.text = $"{entityName}'s Turn.";
            yield return new WaitForSeconds(2.5f);

            if (this.statusEffect != null)
            {
                UpdateStatusEffect(this.statusEffect, this);
                yield return new WaitForSeconds(1.5f);
                if (currentHealth <= 0)
                {
                    StartCoroutine(Die());
                    yield break;
                }
            }

            if (this.isFrozen)
            {
                battleManager.gameText.text = $"{unit.enemyName} is Frozen!";
                yield return new WaitForSeconds(1.5f);
                battleManager.StartNextTurn();
                yield break;
            }
            StartCoroutine(UseMove());

            yield return new WaitUntil(() => doneHit == true);
            battleManager.StartNextTurn();
        }
    }

    public IEnumerator UseMove()
    {
        doneHit = false;
        if (unit.moves.Length == 0)
        {
            battleManager.gameText.text = $"{entityName} has no moves! Poor Guy. ;-;";
            yield break;
        }


        //Code to choose move
        //I am going to implement switch statements to decide move selection when I have time. For now it will be a random selection!
       
        int moveIndex = Random.Range(0, unit.moves.Length);
        UnitMoves selectedMove = unit.moves[moveIndex];
        for (int x = 0; x < selectedMove.hits; x++)
        {
            battleManager.gameText.text = $"{unit.enemyName} used {selectedMove.MoveName}!";
            yield return new WaitForSeconds(1f);
            if (Random.Range(0, 100) <= selectedMove.Accuracy)
            {
                if (selectedMove.Damage > 0)
                {
                    battleManager.gameText.text = $"It hits!";
                    yield return new WaitForSeconds(0.3f);
                    battleManager.GetComponent<BattleManager>().player.TakeDamage(selectedMove.Damage + Attack);
                }

                if (selectedMove.Healing > 0)
                {
                    Heal(selectedMove.Healing, battleManager, healthBar);
                }

                ///
                ///
                ///
                if (selectedMove.StatusCondition != EnumStatusEffect.None)
                {
                  //  SwitchStatusEffects(selectedMove.StatusCondition, battleManager.GetComponent<BattleManager>().player);
                    AddStatusEffect(selectedMove, battleManager.GetComponent<BattleManager>().player, this);
                    yield return new WaitForSeconds(1.5f);
                    battleManager.gameText.text = $"{unit.enemyName} used {selectedMove.MoveName}! Causing {selectedMove.StatusCondition}"; // {}";                 
                }
                ///
                ///
                ///

                moveAnim(selectedMove);

                if (selectedMove.attackSound != null)
                {
                    audioSource.pitch = Random.Range(0.85f, 1.15f);
                    audioSource.PlayOneShot(selectedMove.attackSound);
                }

                if (selectedMove.Damage > 0)
                {
                    StartCoroutine(cameraShake.Shake(0.5f, Random.Range(0.05f, 0.2f)));
                }
                yield return new WaitForSeconds(0.5f);

            }
            else 
            {
                battleManager.gameText.text = $"It misses!";
                yield return new WaitForSeconds(0.5f); 
            }
            hitCount++;
        }
        if (hitCount >= selectedMove.hits) 
        { 
            doneHit = true; 
        }
    }

    public IEnumerator Die()
    {
        //Temp sprite delete, change to death animation
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().enabled = !enabled;

        //Instantiate Money
        battleManager.DropMoney(unit.GoldReward);
        yield return new WaitForSeconds(2f);
        isDead = true;

        //Player Rewards
        battleManager.gameText.text = $"The {entityName} has been slain! You recieved {unit.GoldReward} gold and {unit.EXP} XP!";
        battleManager.GetComponent<BattleManager>().player.Money += unit.GoldReward;
        battleManager.GetComponent<BattleManager>().player.exp += unit.EXP;

        //BattleTracker
        gameManager.battlesWon += 1;
        yield return new WaitForSeconds(2.5f);

        if (gameManager.battlesWon == 12 /*"Number of battles for game"*/)
        {
            gameManager.won = true;
        }
        else
        {
            battleManager.GetComponent<BattleManager>().player.moveUiUnActive();
            SceneManager.LoadScene(2);
        }
    }

    public void moveAnim(UnitMoves usedMove)
    {
        switch (usedMove.attackType)
        {
            case "Kick":
                animator.GetComponent<Animator>().SetTrigger("GetKicked");
                break;
            case "Punch":
                animator.GetComponent<Animator>().SetTrigger("GetPunched");
                break;
            case "Slice":
                animator.GetComponent<Animator>().SetTrigger("Slice");
                break;
            case "Elemental":
                animator.GetComponent<Animator>().SetTrigger("GetElementalAttacked");
                break;
            case "Heal":
                animator.GetComponent<Animator>().SetTrigger("Heal");
                backAnimation.GetComponent<Animator>().SetTrigger("HealBack");
                break;
            default:
                return;
        }
    }
}