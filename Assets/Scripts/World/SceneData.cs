using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class SceneData : ScriptableObject
{
    [SerializeField]
    private int sceneIndex;
    [SerializeField]
    private List<DamagableCharacter> enemies;
    [SerializeField]
    private List<Vector2> enemiesCoordinates;
    [SerializeField]
    private Vector2 spawnCoordinates;
    [SerializeField]
    private bool isCutScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        PlayerEvents.GetInstance().Teleport(spawnCoordinates);
        PlayerController.IsCutScene = isCutScene;
        // TO-DO: Spawn enemies on defined coordinates
    }
}
