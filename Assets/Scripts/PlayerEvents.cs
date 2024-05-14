using System;
using UnityEngine;
public class PlayerEvents
{
    private static PlayerEvents instance;
    public event Action<int> OnExperienceGained, OnHealed;
    public event Action OnGameStart;
    public event Action<Sprite[], int> OnArmourChanged;
    public event Action<Sprite[]> OnWeaponChanged;
    public event Action<bool> OnShieldUse, OnSave;
    public event Action<Vector2> OnTeleported;

    public static IDataService dataService = new JsonDataService("/test_save");
    //public static int currentScene;

    private PlayerEvents(){}

    // Public static method to access the single instance
    public static PlayerEvents GetInstance()
    {
        // Create the instance if it doesn't exist
        if (instance == null)
        {
            instance = new PlayerEvents();
        }
        return instance;
    }

    public void ArmourChanged(Sprite[] sprite, int index)
    {
        OnArmourChanged?.Invoke(sprite, index);
    }

    public void WeaponChanged(Sprite[] sprite)
    {
        OnWeaponChanged?.Invoke(sprite);
    }

    public void ShieldUse(bool trigger)
    {
        OnShieldUse?.Invoke(trigger);
    }

    public void ExperienceGained(int exp)
    {
        OnExperienceGained?.Invoke(exp);
    }
    public void Heal(int amount)
    {
        OnHealed?.Invoke(amount);
    }

    public void GameStarted()
    {
        OnGameStart?.Invoke();
    }

    public void Teleport(Vector2 coordinates)
    {
        OnTeleported?.Invoke(coordinates);
    }

    public void Save(bool savePlayerPos) 
    { 
        OnSave?.Invoke(savePlayerPos);
    }
}
