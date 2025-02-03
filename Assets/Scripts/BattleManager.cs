using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public enum FightState{ Start, Player, Enemy};
    private FightState currentState;

    public GameObject enemyPos;
    public GameObject enemy;
    public GameObject yourEnemy; //THIS IS THE ACTUAL ENEMY REFERENCED FROM ABOVE
    public Enemy enemyScript;

    public CharacterManager player;

    private BattleEntity currentTurn;

    public GameManager gameManager;
    public TMP_Text gameText;
    public UnitMoves usedMove;

    private int enemyScaler;
    void Start()
    {

        gameManager = GameManager.instance;
        player = CharacterManager.instance;

        enemyScaler = gameManager.enemyLevel; //Scales enemies to keep getting stronger

        player.UIActive();
        //code below should change based on Boss Battle or Small Battle
        if (gameManager.battlesWon % 4 != 0 || gameManager.battlesWon == 0)
        {
            enemy = gameManager.GetComponent<GameManager>().battles[0];
            gameManager.battles.Add(enemy);
            gameManager.battles.RemoveAt(0);
        }
        else if(gameManager.battlesWon % 4 == 0)
        {
            enemy = gameManager.GetComponent<GameManager>().BossBattles[0];
            gameManager.battles.Add(enemy);
            gameManager.BossBattles.RemoveAt(0);
        }
        yourEnemy = Instantiate(enemy, enemyPos.transform.position, Quaternion.identity);
        enemyScript = yourEnemy.GetComponent<Enemy>();

        scaleEnemies(enemyScaler);
        level();

        ChangeState(FightState.Start);
    }
    public void ChangeState(FightState newState)
    {
        currentState = newState;


        switch (currentState)
        {
            case FightState.Start:
                Debug.Log("Start");
                gameText.text = $"{enemyScript.unit.enemyName} appeared!";
                SetTurnOrder();
                // StartNextTurn(); //Use this code in other scripts to say when done turn
                break;

            case FightState.Player:
                player.isTurn = true;
                StartCoroutine(player.TakeTurn());
                    //We need something to say when x happens in Character manager then change state. Or find a way to connect battlemanager to player          
                break;

            case FightState.Enemy:
                StartCoroutine(enemyScript.TakeTurn());
                break;
        }
    }
    public void SetTurnOrder()
    {
        if (player.Speed >= enemyScript.Speed)
        {
            ChangeState(FightState.Player);
        }
        else
        {
            ChangeState(FightState.Enemy);
        }
    }

    public void StartNextTurn() //Use when other entity hasn't died
    {
        switch (currentState)
        {
            case FightState.Player:
                ChangeState(FightState.Enemy);
                break;

            case FightState.Enemy:
                ChangeState(FightState.Player);
                break;
        }
    }

    public void StatusCondition()
    {
        
    }

    public void level()
    {
            enemyScript.maxHealth += enemyScript.Level;
            enemyScript.currentHealth = enemyScript.maxHealth;
            enemyScript.Attack += enemyScript.Level;
            enemyScript.Defence += enemyScript.Level;
            enemyScript.Speed += enemyScript.Level;
    }
    public void scaleEnemies(int scale)
    {
        enemyScript.maxHealth *= scale;
        enemyScript.currentHealth = enemyScript.maxHealth;
        enemyScript.Attack *= scale;
        enemyScript.Defence *= scale;
        enemyScript.Speed *= scale;
        enemyScript.Level *= scale;
    }
}