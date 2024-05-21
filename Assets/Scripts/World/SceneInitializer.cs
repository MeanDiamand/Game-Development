using Assets.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private bool isCutScene;

    [SerializeField]
    private SwitchLevel teleport;

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
        TryShowTeleport();
    }

    private void OnDestroy()
    {
        PlayerEvents.GetInstance().OnSave -= Save;
    }

    private void TryShowTeleport()
    {
        if (!isCutScene & alive.Count(b => b) == 0)
            ShowTeleport();
    }

    private void HandleEnemyDestroyed(DamagableCharacter destroyedEnemy)
    {
        enemies.RemoveAt(enemies.IndexOf(destroyedEnemy));
        alive[destroyedEnemy.Index] = false;
        destroyedEnemy.OnDestroyed -= HandleEnemyDestroyed;

        string str = "";
        foreach (bool b in alive)
        {
            if (b) str += "1";
            else str += "0";
        }
        Debug.Log($"Alive: {alive.Count(b => b)} / {str} / {destroyedEnemy.Index}");
        TryShowTeleport();
    }

    private void ShowTeleport() 
    {
        if (teleport == null)
        {
            Debug.LogError("No teleport object!");
            return;
        }
        teleport.Show();
        Debug.Log("Teleport Appeared!");
    }

    private void CreateEnemy(DamagableCharacter prefab, Vector2 coordinates, int index = -1, float health = -1)
    {
        try
        {
            DamagableCharacter enemy = Instantiate(prefab, coordinates, Quaternion.identity);
            enemy.OnDestroyed += HandleEnemyDestroyed;
            if (health > 0)
                enemy.Health = health;
            if (index > 0)
                enemy.Index = index;

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
            CreateEnemy(enemiesPrefabs[i], enemiesSpawnCoordinates[i], i);
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
                CreateEnemy(enemiesPrefabs[i], new Vector2(save.xCords[i], save.yCords[i]), save.indixes[i], save.heath[i]);
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
        save.indixes = new int[n];
        save.xCords = new float[n];
        save.yCords = new float[n];

        for (int i = 0; i < n;i++)
        {
            //if (!alive[i]) continue;

            save.heath[i] = enemies[i].Health;
            save.indixes[i] = enemies[i].Index;

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
        public int[] indixes;

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
