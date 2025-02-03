using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : BattleEntity
{
    public GameManager gameManager;
    public BattleManager battleManager;
    public Unit unit;
    public bool usedMove = false;
    private bool isDead = false;

    public Slider healthBar;

    public GameObject animator;
    public GameObject backAnimation;
    public CameraShake cameraShake;
    public AudioSource audioSource;
    public bool cursed = false;

    public int hitCount = 0;
    private bool doneHit = false;
    void Start()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        healthBar = GameObject.FindGameObjectWithTag("enemyHealth").GetComponent<Slider>();
        gameManager = GameManager.instance;
        animator = GameObject.FindGameObjectWithTag("enemyAnimator");
        backAnimation = GameObject.FindGameObjectWithTag("healBackAnimator");
        cameraShake = FindFirstObjectByType<CameraShake>();

        entityName = unit.enemyName;
        maxHealth = unit.MaxHealth;
        currentHealth = maxHealth;
        Level = unit.Level;
        Attack = unit.Attack;
        Defence = unit.Defence;
        Speed = unit.Speed;

        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();

    }

    private void Update()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    public void TakeDamage(int dmg)
    {
        int EffectiveDamage = Mathf.CeilToInt(dmg - Defence);
        if (EffectiveDamage <= 0)
        {
            EffectiveDamage = 1;
        }
        battleManager.gameText.text = $"The {entityName} took {EffectiveDamage} damage!";
        currentHealth -= EffectiveDamage;
        if (currentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(Die());
        }

        healthBar.value = currentHealth;
    }

    public void Heal(int amount)
    {
        if (currentHealth + (amount * maxHealth) / 100 <= maxHealth)
        {
            battleManager.gameText.text = $"{entityName} healed {(amount * maxHealth) / 100} health";
        }
        else if (currentHealth + (amount * maxHealth) / 100 > maxHealth)
        {
            battleManager.gameText.text = $"{entityName} healed {(maxHealth - currentHealth).ToString()} health";
        }

        currentHealth = Mathf.Min(currentHealth + ((amount * maxHealth)/100), maxHealth);
        healthBar.value = currentHealth;
    }

    public override IEnumerator TakeTurn()
    {
        audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();
        if (!isDead)
        {
            if (cursed)
            {
                StartCoroutine(Cursed());
            }

            battleManager.gameText.text = $"{entityName}'s Turn.";
            yield return new WaitForSeconds(2.5f);
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
        //It is something I need to learn to do but I may not have time before the deadline
       
        int moveIndex = Random.Range(0, unit.moves.Length);
        UnitMoves selectedMove = unit.moves[moveIndex];
        for (int x = 0; x < selectedMove.hits; x++)
        {
            if (Random.Range(0, 100) <= selectedMove.Accuracy)
            {
                if (selectedMove.Damage > 0)
                {
                    battleManager.gameText.text = $"{unit.enemyName} used {selectedMove.MoveName}!";
                    yield return new WaitForSeconds(2f);
                    battleManager.GetComponent<BattleManager>().player.TakeDamage(selectedMove.Damage + Attack);
                }

                if (selectedMove.Healing > 0)
                {
                    Heal(selectedMove.Healing);
                }

                   if (selectedMove.StatusCondition != "")
                   {
                    battleManager.gameText.text = $"{unit.enemyName} used {selectedMove.MoveName}! Causing {statusConditions(selectedMove.StatusCondition)}";
                    battleManager.gameText.text = $"The Status Condition feature hasn't been implemented yet!";
                }

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
            hitCount++;
        }
        if (hitCount >= selectedMove.hits) 
        { 
            doneHit = true; 
        }
    }

    public IEnumerator waitASec(float x)
    {
        Debug.Log("We Waiting");
        yield return new WaitForSeconds(x);
    }
    public string statusConditions(string statCon)
    {
        return statCon;
    }
    public IEnumerator Die()
    {
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

    public IEnumerator Cursed()
    {
        yield return new WaitForSeconds(1.5f);
        currentHealth = Mathf.RoundToInt(currentHealth * 0.85f);
    }
}