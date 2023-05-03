using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    Canvas canvas;
    Canvas pistolCanvas;
    Canvas hudCanvas;
    PlayerMove playerMove;
    MouseLook playerLook;
    PlayerHealth playerHealth;
    Pistol playerGun;
    

    [SerializeField] private GameObject pausePanel, optionsPanel, respawnPanel, mainMenuPanel, quitPanel, deathPanel;

    bool paused;
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        canvas = GetComponent<Canvas>();
        pistolCanvas = FindObjectOfType<Pistol>().gameObject.GetComponentInChildren<Canvas>();
        hudCanvas = FindObjectOfType<UIManager>().GetComponent<Canvas>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerLook = FindObjectOfType<MouseLook>();
        playerGun = FindObjectOfType<Pistol>();
    }

    void Update()
    {
        CheckForPause();

        if(playerHealth.isDead)
        {
            DeathMenu();
        }
    }

    void CheckForPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !paused && playerGun.allowedToFire && !playerHealth.isDead)
        {
            PauseGame();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && paused && !playerHealth.isDead)
        {
            ResumeGame();
        }

    }

    public void PauseGame()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(true);
        pistolCanvas.gameObject.SetActive(false);
        hudCanvas.gameObject.SetActive(false);
        StopActions();
    }

    public void ResumeGame()
    {
        deathPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        respawnPanel.SetActive(false);
        optionsPanel.SetActive(false);


        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanel.SetActive(false);
        pistolCanvas.gameObject.SetActive(true);
        hudCanvas.gameObject.SetActive(true);
        ResumeActions();
    }

    public void PauseMenu()
    {
        deathPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        respawnPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OptionsMenu()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);    
    }

    public void RespawnMenu()
    {
        pausePanel.SetActive(false);
        respawnPanel.SetActive(true);
    }

    public void MainMenu()
    {
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitMenu()
    {
        pausePanel.SetActive(false);
        quitPanel.SetActive(true);
    }

    public void DeathMenu()
    {
        PauseGame();
        pausePanel.SetActive(false);
        deathPanel.SetActive(true);
    }

    void StopActions()
    {
        Time.timeScale = 0f;
        playerMove.isPaused = true;
        playerLook.isPaused = true;
        playerGun.isPaused = true;
    }

    void ResumeActions()
    {
        Time.timeScale = 1f;
        playerMove.isPaused = false;
        playerLook.isPaused = false;
        playerGun.isPaused = false;
    }

    public void ResetScene()
    {
        playerHealth.isDead = false;
        ResumeActions();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        playerHealth.isDead = false;
        ResumeActions();
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitToDesktop()
    {
        playerHealth.isDead = false;
        ResumeActions();
        Application.Quit();
    }


}
