using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CameraRotation cameraRotation;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject controlsMenuUI;
    [SerializeField] private Image gameTitle;
    [SerializeField] private Button resumeButton;
    [SerializeField] private TMP_Text playButton;
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_Text sensitivityNumber;
    [SerializeField] private Slider sensitivitySlider;

    public bool isGameFinished = false;
    public bool isPaused = false;


    void Start()
    {
        gameText.text = " ";
        CursorLock();
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        gameTitle.enabled = true;

        resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
        sensitivitySlider.value = cameraRotation.mouseSensitivity;
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        UpdateSensitivity(sensitivitySlider.value);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                GameManager.Instance.ResumeGame();
                CursorLock();
                pauseMenuUI.SetActive(false);
            }
            else
            {
                GameManager.Instance.PauseGame();
                CursorUnlock();
                pauseMenuUI.SetActive(true);

            }
        }
    }

    public void ShowLoseScreen()
    {
        
        pauseMenuUI.SetActive(true);
        gameTitle.enabled = false;
        playButton.text = "Try Again";
        gameText.text = "You Died";
        CursorUnlock();
    }
    public void ShowWinScreen()
    {
        pauseMenuUI.SetActive(true);
        gameTitle.enabled = false;
        playButton.text = "Play Again";
        gameText.text = "You Escaped";
        CursorUnlock();
    }
    public void ResumeGame()
    {
        if (!isGameFinished)
        {
            GameManager.Instance.ResumeGame();
            pauseMenuUI.SetActive(false);

        }
        else
        {
            GameManager.Instance.Restart();
        }
    }

    public void MenuExit()
    {
        Application.Quit();
    }
    public void OpenSettings()
    {
        settingsMenuUI.SetActive(true);

    }
    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);

    }
    public void UpdateSensitivity(float value)
    {
        cameraRotation.mouseSensitivity = value;

        float precentage = Mathf.Round((value / 100f) * 100f);
        sensitivityNumber.text = precentage + "%";

    }
    public void OpenControls()
    {
        controlsMenuUI.SetActive(true);

    }
    public void CloseControls()
    {
        controlsMenuUI.SetActive(false);

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