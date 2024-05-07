using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    // Method for starting the game
    public void StartGame()
    {
        Time.timeScale = 1;
        //HealthController.health = 5;
        PlayerEvents.GetInstance().GameStarted();
        SceneManager.LoadScene("InitializationScene", LoadSceneMode.Single);
    }

    // Methoda for loading
    public void LoadGame()
    {
        try
        {
            PlayerEvents.dataService.LoadData<int>("/scene_id");
        }
        catch
        {
            //No save, block resume button
        }
    }
}
