using System;
using System.Collections;
using System.Collections.Generic;
public class PlayerEvents
{
    private static PlayerEvents instance;
    public event Action<int> OnExperienceGained, OnHealed;
    public event Action OnGameStart;

    private PlayerEvents()
    {
        // Constructor implementation
    }

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
}
