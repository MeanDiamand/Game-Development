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
            isGameOver = false;
        }
    }

    // Method for restarting the game
    public void RestartGame()
    {
        Time.timeScale = 1;
        //HealthController.health = 5;
        gameOverScreen.SetActive(false);
        PlayerEvents.GetInstance().GameStarted();
        SceneManager.LoadScene("InitializationScene", LoadSceneMode.Single);
    }

    // Method for going back to Menu
    public void Home()
    {
        gameOverScreen.SetActive(false);

        SceneManager.LoadScene("Menu");
    }
}
