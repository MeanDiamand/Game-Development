using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverEvents : MonoBehaviour
{
    public static bool isGameOver;
    public GameObject gameOverScreen;

    private void Awake()
    {
        isGameOver = false;  
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver) 
        {
            gameOverScreen.SetActive(true);    
        }
    }

    // Method for restarting the game
    public void RestartGame()
    {
        Time.timeScale = 1;
        //HealthController.health = 5;
        PlayerEvents.GetInstance().GameStarted();
        SceneManager.LoadScene("SampleScene");
    }

    // Method for going back to Menu
    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
}
