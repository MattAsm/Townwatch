using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuOptions : MonoBehaviour
{
    public CharacterManager characterManager;
    public GameManager gameManager;

    private bool tutOpen = false;
    public GameObject tutorial;
    public void Start()
    {
        characterManager = CharacterManager.instance;
        gameManager = GameManager.instance;

        if(characterManager != null)
        {
            Destroy(characterManager.gameObject);
        }
        if(gameManager != null)
        {
            Destroy(gameManager.gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting... Supposedly");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenTutorial()
    {
    if(!tutOpen)
        {
            tutorial.SetActive(true);
            tutOpen = true;
        }
        else
        {
            tutorial.SetActive(false);
            tutOpen = false;
        }
    }
}
