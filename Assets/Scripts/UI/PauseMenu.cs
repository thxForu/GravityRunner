using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel, homeCanvas, gameCanvas, continueButton, restartButton;
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        gameCanvas.SetActive(true);
        pauseMenuPanel.SetActive(false);
        homeCanvas.SetActive(false);
    }

    public void HomeButton()
    {
        homeCanvas.SetActive(true);
        pauseMenuPanel.SetActive(false);
        continueButton.SetActive(true);
        restartButton.SetActive(false);
        gameCanvas.SetActive(false);
        
    }
}