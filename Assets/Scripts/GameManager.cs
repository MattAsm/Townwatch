using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharacterManager characterManager;

    public int battlesWon = 0;

    public List<GameObject> battles; //Sets up the battles
    public List<GameObject> BossBattles;

    private List<GameObject> battlesCont;
    private List<GameObject> BossBattleCont;
    private int sceneNum; //Temporary to control scene management and scene movement before having implemented proper scene changing

    public GameObject wonUI;

    public List<GameObject> uiElementsToHide;

    public bool won = false;

    [HideInInspector] public int enemyLevel = 1;
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

        characterManager = CharacterManager.instance;

        battlesCont = battles;
        BossBattleCont = BossBattles;
    } //Manager Stuff


    private void Update()
    {
        NextScene(); //For changing scene with space bar
    /*  if(battles.Count <= 0)
        {
           battles = battlesCont;
            enemyLevel += 1;
        }
      if(BossBattles.Count <= 0)
        {
            BossBattles = BossBattleCont;
            enemyLevel += 10;
        }
    */

        if (battlesWon == 12 && won /*"Number of battles for game"*/)
        {
                WonGameUI();
        }
    }
    private void NextScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sceneNum < 3)
            {
                sceneNum++;
            }
            else
                sceneNum = 0;

            SceneManager.LoadScene(sceneNum);
        }
    } //Temp scene switcher

    public void WonGameUI()
    {
        wonUI.SetActive(true);
        foreach(GameObject ui in uiElementsToHide)
        {
            if (ui != null) {
                ui.SetActive(false);
            }
        }
        Time.timeScale = 0.1f;
    }

    public void ContinuePlaying()
    {
        won = false;
       
        Time.timeScale = 1f;
        wonUI.SetActive(false);
        SceneManager.LoadScene(2);
    }
    public void ReturnToMenu()
    {
        won = false;
        Time.timeScale = 1f;
        wonUI.SetActive(false);
        characterManager.moveUiUnActive();
        SceneManager.LoadScene(0);
    }

}
