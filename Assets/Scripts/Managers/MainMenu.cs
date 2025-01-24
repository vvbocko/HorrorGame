using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    //    }
    //}

    public void MenuPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //or just 1
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
