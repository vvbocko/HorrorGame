using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Referencja do Canvas z menu pauzy
    public Button resumeButton;     // Referencja do przycisku "Resume"

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }
    public void MenuExit()
    {
        Application.Quit();
    }
    public void MenuSettings()
    {
        //sth
    }
}