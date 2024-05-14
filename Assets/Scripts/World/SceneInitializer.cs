using Assets.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;
    [SerializeField]
    private List<DamagableCharacter> enemiesPrefabs;
    [SerializeField]
    private List<Vector2> enemiesSpawnCoordinates;
    [SerializeField]
    private Rigidbody2D player;

    private List<DamagableCharacter> enemies = new List<DamagableCharacter>();

    //[SerializeField]
    //private Vector2 playerSpawn;

    [SerializeField]
    private bool isCutScene;

    private bool[] alive;

    private void Start()
    {
        PlayerController.IsCutScene = isCutScene;
        PlayerEvents.GetInstance().OnSave += Save;

        GameObject canvas = GameObject.Find("MainCanvas");
        if (canvas != null)
        {
            canvas.SetActive(!isCutScene);
            Debug.Log($"MainCanvas visibility set to {!isCutScene}");
        }

        Load();
    }

    private void OnDestroy()
    {
        PlayerEvents.GetInstance().OnSave -= Save;
    }

    private void HandleEnemyDestroyed(DamagableCharacter destroyedEnemy)
    {
        int index = enemies.IndexOf(destroyedEnemy);
        if (index < 0) Debug.LogError("DamagableCharacter is not in enemy list!");
        enemies.RemoveAt(index);
        alive[index] = false;
        destroyedEnemy.OnDestroyed -= HandleEnemyDestroyed;
    }

    private void CreateEnemy(DamagableCharacter prefab, Vector2 coordinates, float health = -1)
    {
        try
        {
            DamagableCharacter enemy = Instantiate(prefab, coordinates, Quaternion.identity);
            enemy.OnDestroyed += HandleEnemyDestroyed;
            if (health > 0)
                //enemy.SetHealth(health);
                enemy.Health = health;
            enemies.Add(enemy);
        }
        catch (Exception ex)
        {
            // Catch and log the exception details for debugging
            Debug.LogError($"Exception occurred while instantiating enemy with health: {health}");
            Debug.LogError($"Exception occurred while instantiating enemy: {ex.Message}");
        }
    }

    private void Load()
    {
        try
        {
            SceneSave save = PlayerEvents.dataService.LoadData<SceneSave>($"/scene_{sceneIndex}");
            LoadFile(save);
        }
        catch
        {
            LoadDefault();
        }
    }

    private void LoadDefault()
    {
        if (enemiesPrefabs.Count != enemiesSpawnCoordinates.Count)
        {
            Debug.LogError("Lengths of enemiesPrefabs and enemiesSpawnCoordinates are not equal!");
            return;
        }
        for (int i = 0; i < enemiesPrefabs.Count; i++)
        {
            CreateEnemy(enemiesPrefabs[i], enemiesSpawnCoordinates[i]);
        }

        alive = new bool[enemiesPrefabs.Count];
        for (int i = 0; i < alive.Length; i++)
            alive[i] = true;
    }

    private void LoadFile(SceneSave save)
    {
        if (enemiesPrefabs.Count != save.alive.Length)
        {
            Debug.LogError("Lengths of enemiesPrefabs and save.alive are not equal!");
            return;
        }
        for (int i = 0; i < save.alive.Length; i++)
        {
            if (save.alive[i])
                CreateEnemy(enemiesPrefabs[i], new Vector2(save.xCords[i], save.yCords[i]), save.heath[i]);
        }
        alive = save.alive;

        if (player != null & save.savePlayerPos)
            player.position = new Vector2(save.playerX, save.playerY);
    }

    private void Save(bool savePlayerPos)
    {
        SceneSave save = new SceneSave();
        save.alive = alive;
        
        int n = enemies.Count;
        save.heath = new float[n];
        save.xCords = new float[n];
        save.yCords = new float[n];

        for (int i = 0; i < n;i++)
        {
            save.heath[i] = enemies[i].Health;

            Vector2 coordinate = enemies[i].GetCoordinates();
            save.xCords[i] = coordinate.x;
            save.yCords[i] = coordinate.y;
        }

        if (player != null)
        {
            save.playerX = player.position.x;
            save.playerY = player.position.y;
        }
        save.savePlayerPos = savePlayerPos;

        PlayerEvents.dataService.SaveData($"/scene_{sceneIndex}", save);


        PlayerEvents.dataService.SaveData("/scene_id", sceneIndex);
    }


    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SceneSave
    {
        [JsonProperty]
        public bool[] alive;

        [JsonProperty]
        public float[] heath;

        [JsonProperty]
        public float[] xCords;

        [JsonProperty]
        public float[] yCords;

        [JsonProperty]
        public bool savePlayerPos;
        [JsonProperty]
        public float playerX;
        [JsonProperty]
        public float playerY;
    }
}
