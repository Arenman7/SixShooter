using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainScreen, optionsScreen, creditsScreen, quitScreen;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsMenu()
    {
        mainScreen.gameObject.SetActive(false);
        optionsScreen.gameObject.SetActive(true);
    }

    public void CreditsMenu()
    {
        mainScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(true);
    }

    public void QuitMenu()
    {
        mainScreen.gameObject.SetActive(false);
        quitScreen.gameObject.SetActive(true);
    }

    public void ReturnToMenu()
    {
        mainScreen.gameObject.SetActive(true);
        optionsScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);
        quitScreen.gameObject.SetActive(false);
    } 

    public void QuitGame()
    {
        Application.Quit();
    }
}
