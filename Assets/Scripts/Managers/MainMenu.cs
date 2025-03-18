using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject controlsMenuUI;
    [SerializeField] private TMP_Text playButton;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    //    }
    //}
    void Start()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        playButton.text = "PLAY";
    }
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
    public void OpenControls()
    {
        controlsMenuUI.SetActive(true);

    }
    public void CloseControls()
    {
        controlsMenuUI.SetActive(false);

    }
}
