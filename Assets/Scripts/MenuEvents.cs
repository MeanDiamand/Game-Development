using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuEvents : MonoBehaviour
{
    [SerializeField]
    private Button btnLoadGame;

    private int savedLevel = 2;

    private void Start()
    {
        try
        {
            savedLevel = PlayerEvents.dataService.LoadData<int>("/scene_id");
        }
        catch
        {
            btnLoadGame.interactable = false;
        }
    }

    /// <summary>                        
    /// Method to exit the game
    /// /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    /// <summary>                        
    /// Method for starting new game
    /// /// </summary>
    public void StartGame()
    {
        PlayerEvents.dataService.DeleteFiles();
        StartLevel(3);
    }

    /// <summary>                        
    /// Method for loading existing game
    /// /// </summary>
    public void LoadGame()
    {
        StartLevel(savedLevel);
    }

    public void StartLevel(int id)
    {
        Time.timeScale = 1;
        PlayerEvents.GetInstance().GameStarted();
        SceneManager.LoadScene(id, LoadSceneMode.Single);
    }
}
