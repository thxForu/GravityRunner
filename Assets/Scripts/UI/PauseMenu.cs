using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    public GameObject HomeCanvas;
    public GameObject GameCanvas;
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenuPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        GameCanvas.SetActive(true);
        PauseMenuPanel.SetActive(false);
        HomeCanvas.SetActive(false);
    }

    public void HomeButton()
    {
        HomeCanvas.SetActive(true);
        PauseMenuPanel.SetActive(false);
        GameCanvas.SetActive(false);
    }
}