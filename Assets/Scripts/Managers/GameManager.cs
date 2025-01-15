using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int someData = 99;

    private int totalMiniGames = 2;
    private int completedMiniGames = 0;


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
            WinGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("Wszystkie minigierki ukonczone");
    }
    private void LoseGame()
    {
        Debug.Log("Wszystkie minigierki ukonczone");
    }
}
