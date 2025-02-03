using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;
using UnityEngine.SceneManagement;
using System.Data.SqlTypes;

public class CharacterManager : BattleEntity
{
    public int Accuracy;

    public BattleManager battleManager;
    public static CharacterManager instance;
    public Unit unit;
    public List<UnitMoves> learntMoves;
    public List<UnitMoves> selectedMoves;
    public int moveNum;

    public List<Button> moveButtons;
    public List<TMP_Text> moveNames;

    public bool usedMove = false;
    public bool isTurn = false;

    private bool moveUiActive;
    [SerializeField]private GameObject PlayerCanvas; 
    public Slider healthBar;

    [Header("Rounds")]
    public TMP_Text roundCount;
    public int round;
    [Header("Gold")]
    public TMP_Text moneyDisplay;
    public int Money = 0;
    [Header("Stats Display")]
    public GameObject statsToDisplay;
    public TMP_Text statsOnDisplay;
    public bool statsDisplaying = false;
    [Header("EXP")]
    public int exp = 0;
    private int oldExpReq = 0;
    public int expToLevel = 100;
    public Slider expBar;

    [HideInInspector] public Weapon weaponEquipped;

    public GameObject animator;
    public AudioSource audioSource;

    public bool cursed = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    } //Manager Stuff

    void Start()
    {
        round = 1;
        uiMoves();

        entityName = unit.enemyName;
        maxHealth = unit.MaxHealth;
        currentHealth = maxHealth;
        Level = unit.Level;
        Attack = unit.Attack;
        Defence = unit.Defence;
        Speed = unit.Speed;

        Accuracy = 100;

        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();

    }

  
    void Update()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //XP Stuff
        expBar.minValue = oldExpReq;
        expBar.maxValue = expToLevel;
        expBar.value = exp;
        roundCount.text = $"Round: {round.ToString()}";
        moneyDisplay.text = $"Gold:{Money.ToString()}";
        PreventNegatives();
        if(isTurn)
        {
            UseMove();
        }

        if (SceneManager.GetActiveScene().name == "FightSceneFP" && moveUiActive == false)
        {
            moveUiActive = true;
            UIActive();
        }

        if(exp >= expToLevel)
        {
            LevelUp();          
        }
    }

    public void UseMove()
    {
        if (unit.moves.Length == 0)
        {
            Debug.Log($"{unit.enemyName} has no moves! :(");
            return;
        } //Checks if player has moves equipped

        if (isTurn == true)
        {
            isTurn = false;
            foreach (Button button in moveButtons)
            {  
                button.onClick.AddListener(() => usedMove = true);
            }
            moveButtons[0].onClick.AddListener(() => moveNum = 0);
            moveButtons[1].onClick.AddListener(() => moveNum = 1);
            moveButtons[2].onClick.AddListener(() => moveNum = 2);
            moveButtons[3].onClick.AddListener(() => moveNum = 3);
        }
    }
  
    public override IEnumerator TakeTurn()
    {
        audioSource = GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>();
        if (cursed) {
            StartCoroutine(Cursed());
        }
        yield return new WaitForSeconds(1.5f);
        battleManager = FindAnyObjectByType<BattleManager>();
        battleManager.gameText.text = "Your turn";

        yield return new WaitUntil(() => usedMove);
        foreach (Button button in moveButtons) //RemovesListeners to stop multihitting buttons
        {
            button.onClick.RemoveAllListeners();
        }//Clears Buttons
        usedMove = false;
        for (int x = 0; x < selectedMoves[moveNum].hits; x++) 
        {
            if(battleManager.GetComponent<BattleManager>().enemyScript.GetComponent<Enemy>().currentHealth <=0)
            {
                break;
            }
            if (doesHit())
            {
                StartCoroutine(MoveEffects());
            }
            else
            {
                battleManager.gameText.text = $"You Missed!";
            }
            yield return new WaitForSeconds(0.75f);
        }
        yield return new WaitForSeconds(2f);   //For now simulating with a waitforseconds but change to waiting for animation to complete

        battleManager.StartNextTurn();
    }

    public bool doesHit()
    {
        if (Random.Range(0, 100) <= selectedMoves[moveNum].Accuracy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Heal(int amount)
    {
        if (currentHealth + amount <= maxHealth)
        {
            battleManager.gameText.text = $"You healed {currentHealth + ((amount * maxHealth) / 100)} health";
        }
        else if (currentHealth + amount > maxHealth)
        {
            battleManager.gameText.text = $"You healed {(maxHealth - currentHealth).ToString()} health";
        }
        currentHealth = Mathf.Min(currentHealth + ((amount * maxHealth) / 100), maxHealth);
        healthBar.value = currentHealth;
    }
    public IEnumerator MoveEffects()
    {
        //If Move Damages
        if (selectedMoves[moveNum].Damage > 0)
        {
            battleManager.gameText.text = $"You used {selectedMoves[moveNum].MoveName} on the {battleManager.GetComponent<BattleManager>().enemyScript.GetComponent<Enemy>().entityName}!";
            yield return new WaitForSeconds(0.5f);
            battleManager.GetComponent<BattleManager>().enemyScript.GetComponent<Enemy>().TakeDamage(selectedMoves[moveNum].Damage + Attack);
        }
        //If Move Heals
        if (selectedMoves[moveNum].Healing > 0)
        {
            Heal(selectedMoves[moveNum].Healing);
        }
        //If Move Causes Status Effect
         if (selectedMoves[moveNum].StatusCondition != "")
         {
            battleManager.gameText.text = $"The Player used {selectedMoves[moveNum]} causing the status condition {statusConditions(selectedMoves[moveNum].StatusCondition)}";
            battleManager.gameText.text = $"The Status Condition feature hasn't been implemented yet!";
        }

        moveAnim();
        if (selectedMoves[moveNum].attackSound != null) 
        {
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(selectedMoves[moveNum].attackSound);
        }
    }
    public UnitMoves getMove(int x) //Grabs the selected move based on List Value
    {
        Debug.Log($"Move:{selectedMoves[x].MoveName} | Damage:{selectedMoves[x].Damage} | Healing:{selectedMoves[x].Healing} | Status Condition:{selectedMoves[x].StatusCondition} | Accuracy:{selectedMoves[x].Accuracy}");
        return selectedMoves[x];
    }
    public void TakeDamage(int dmg)
    {
        int EffectiveDamage = Mathf.CeilToInt(dmg - Defence);
        if (EffectiveDamage <= 0)
        {
            EffectiveDamage = 1;
        }
        currentHealth -= EffectiveDamage;
       
        battleManager.gameText.text = $"You took {EffectiveDamage} damage!";
      
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
        healthBar.value = currentHealth;
    }

    public IEnumerator Die()
    {
        battleManager.gameText.text = "You've been slain";
        moveUiUnActive(); //Turns off Moves Select
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(3);
    }

    public string statusConditions(string statCon)
    {
        return null;
    }

    public void uiMoves()
    {
        //Use to change order of moves
        //Call this when changing move order or selection

        //MOVE 1
        moveNames[0].text = selectedMoves[0].MoveName;

        //MOVE 2
        moveNames[1].text = selectedMoves[1].MoveName;

        //MOVE 3
        moveNames[2].text = selectedMoves[2].MoveName;

        //MOVE 4
        moveNames[3].text = selectedMoves[3].MoveName;
    }

    public void UIActive()
    {
        PlayerCanvas.gameObject.SetActive(true);
    }
    public void moveUiUnActive()
    {
        PlayerCanvas.gameObject.SetActive(false);
    }

    private void PreventNegatives()
    {
        if (Attack < 0)
        {
            Attack = 0;
        }
        if (Defence < 0)
        {
            Defence = 0;
        }
        //Ignoring health stat cause if health is or less then 0 then you are dead!
    }//Prevents stats from going in the negative

    private void LevelUp() //I want leveling up to give some sort of RNG. Maybe 4 random stats gain 1 - 2 points a level or you select where to put them
    {
        oldExpReq = expToLevel;
        if (Level <= 10)
        {
            expToLevel += 20;
        }
        else if (Level > 10 && Level <= 30)
        {
            expToLevel = Mathf.RoundToInt(100 * Mathf.Pow(1.15f, Level));
        }
        else if (Level > 30)
        {
            expToLevel = Mathf.RoundToInt(1.33f * expToLevel);
        }

        Level += 1;
        maxHealth += 15;
        currentHealth += 15;
        Attack += 1;
        Defence += 1;
        Speed += 1; 
    }

    public void statsDisplay()
    {
        if (!statsDisplaying)
        {
            statsToDisplay.SetActive(true);
            statsOnDisplay.text = $"Stats <br>Level: {Level}<br>XP: {exp}<br>XP to next level: {expToLevel}<br>Max Health: {maxHealth}<br>Attack: {Attack}<br>Defence: {Defence}<br>Speed: {Speed}";
            statsDisplaying = true;
        }
        else if (statsDisplaying)
        {
            statsToDisplay.SetActive(false);
            statsDisplaying = false;
        }
    } //Button

    public void weaponEquip(Weapon weapon)
    {
        weaponEquipped = weapon;
        Attack += weaponEquipped.AttackPower;
        Speed += weaponEquipped.Speed;
    }
    public void unequipWeapon()
    {
        Attack -= weaponEquipped.AttackPower;
        Speed -= weaponEquipped.Speed;
        weaponEquipped = null;
    }

    public IEnumerator Cursed()
    {
        yield return new WaitForSeconds(1.5f);
        battleManager.gameText.text = $"You took {currentHealth * 0.15f} damage due to a curse";
        currentHealth = Mathf.RoundToInt(currentHealth * 0.85f);  
    }

    public void moveAnim()
    {
        switch (selectedMoves[moveNum].attackType)
        {
            case "Kick":
                animator.GetComponent<Animator>().SetTrigger("Kick");
                break;
            case "Punch":
                animator.GetComponent<Animator>().SetTrigger("Punch");
                break;
            case "Slice":
                animator.GetComponent<Animator>().SetTrigger("Slice");
                break;
            case "Elemental":
                animator.GetComponent<Animator>().SetTrigger("Elemental");
                break;
            case "Heal":
                animator.GetComponent<Animator>().SetTrigger("PlayerHeal");
                break;
            default:
                return;
        }

    }
}