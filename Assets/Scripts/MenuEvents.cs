using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    // Method for starting the game
    public void StartGame(int index)
    {
        Time.timeScale = 1;
        HealthController.health = 5;
        SceneManager.LoadScene(index);
    }
}
