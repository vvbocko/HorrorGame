using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private TMP_Text playButton;
    [SerializeField] private TMP_Text title;

    private bool isPaused = false;
    private bool isLosed = false;

    void Start()
    {
        CursorLock();
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
                CursorLock();
            }
            else
            {
                PauseGame();
                CursorUnlock();

            }
        }
    }
    public void LoseGame()
    {
        isLosed = true;
        StartCoroutine(ShowLoseScreen());
    }

    private IEnumerator ShowLoseScreen()
    {
        yield return new WaitForSeconds(3f);

        isPaused = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
        playButton.text = "Try Again";
        title.text = "You Died";
        CursorUnlock();
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }
    public void ResumeGame()
    {
        if (!isLosed)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
        }
        else
        Restart();
    }
    void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void MenuExit()
    {
        Application.Quit();
    }
    public void MenuSettings()
    {
        //sth
    }
    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void CursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}