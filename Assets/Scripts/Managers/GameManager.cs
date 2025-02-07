using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int someData = 99;

    private int totalMiniGames = 2;
    private int completedMiniGames = 0;

    [SerializeField] private WinninGame winningGame;
    [SerializeField] private PauseMenu pauseMenu;

    public bool isGameFinished = false;
    public bool isPaused = false;
    public void SayHello()
    {
        Debug.LogFormat("Hello, I'm Game Manager! someData value: {0}", someData);
    }

    public void CompleteOneMiniGame()
    {
        completedMiniGames++;
        Debug.LogFormat("ukoñczno {0}/{1} minigierek", completedMiniGames, totalMiniGames);

        if (completedMiniGames >= totalMiniGames)
        {
            winningGame.MoveContainer();
        }
    }

    public void WinGame()
    {
        completedMiniGames = 0;       
        isGameFinished = true;
        pauseMenu.isGameFinished = isGameFinished;
        pauseMenu.ShowWinScreen();
        PauseGame();

    }
    public void LoseGame()
    {
        completedMiniGames = 0;
        isGameFinished = true;
        pauseMenu.isGameFinished = isGameFinished;
        StartCoroutine(WaitAndShowLoseScreen());

    }
    private IEnumerator WaitAndShowLoseScreen()
    {
        yield return new WaitForSeconds(2.8f);
        pauseMenu.ShowLoseScreen();
        PauseGame();
    }
    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.isPaused = isPaused;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.isPaused = isPaused;
        Time.timeScale = 1;
    }
    public void Restart()
    {
        isPaused = false;
        isGameFinished = true;

        pauseMenu.isPaused = isPaused;
        pauseMenu.isGameFinished = isGameFinished;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
