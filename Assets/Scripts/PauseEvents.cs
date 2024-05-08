using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseEvents : MonoBehaviour
{
    public GameObject pauseMenu;
    // Method for pausing the game
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    // Method for resuming the game
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    // Method for saving the game
    public void SaveGame()
    {
        PlayerEvents.GetInstance().Save();
    }

    // Method for going back to Menu
    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
}
