using System;
using UnityEngine;
using static CharacteristicsUI;

[CreateAssetMenu]
public class PlayerCharacteristics : ScriptableObject
{
    private static int MAX_CHARACTERISTIC_VALUE = 10;
    private static int[] EXPERIENCE_FOR_LVL = new int[] { 1, 2, 4, 8, 16, 32, 64 };
    public int MaxExp()
    {
        if (_level < EXPERIENCE_FOR_LVL.Length)
            return EXPERIENCE_FOR_LVL[_level];
        else return EXPERIENCE_FOR_LVL[EXPERIENCE_FOR_LVL.Length];
    }

    [SerializeField]
    private int _level;
    public int Level
    {
        get { return _level; }
        private set { _level = value; }
    }

    [SerializeField]
    private int _experience;
    public int Experience
    {
        get { return _experience; }
        private set { _experience = value; }
    }

    [SerializeField]
    private int _availablePoints;
    public int AvailablePoints
    {
        get { return _availablePoints; }
        private set { _availablePoints = value; }
    }

    [SerializeField]
    private int _strength;
    public int Strength
    {
        get { return _strength; }
        private set { _strength = value; }
    }

    [SerializeField]
    private int _endurance;
    public int Endurance
    {
        get { return _endurance; }
        private set { _endurance = value; }
    }

    [SerializeField]
    private int _intelligence;
    public int Intelligence
    {
        get { return _intelligence; }
        private set { _intelligence = value; }
    }

    [SerializeField]
    private int _agility;
    public int Agility
    {
        get { return _agility; }
        private set { _agility = value; }
    }

    [SerializeField]
    private int _luck;
    public int Luck
    {
        get { return _luck; }
        private set { _luck = value; }
    }

    public event Action<PlayerCharacteristics> OnUpdated;

    public PlayerCharacteristics()
    {
        PlayerEvents.GetInstance().OnExperienceGained += GainExperience;
    }

    public static PlayerCharacteristics CreateFrom(PlayerCharacteristics original)
    {
        PlayerCharacteristics copy = ScriptableObject.CreateInstance<PlayerCharacteristics>();

        copy._level = original.Level;
        copy._experience = original.Experience;
        copy._availablePoints = original.AvailablePoints;

        copy._strength = original.Strength;
        copy._endurance = original.Endurance;
        copy._intelligence = original.Intelligence;
        copy._agility = original.Agility;
        copy._luck = original.Luck;

        return copy;
    }

    public void SetData(PlayerCharacteristics source)
    {
        this._level = source.Level;
        this._experience = source.Experience;
        this._availablePoints = source.AvailablePoints;

        this._strength = source.Strength;
        this._endurance = source.Endurance;
        this._intelligence = source.Intelligence;
        this._agility = source.Agility;
        this._luck = source.Luck;
    }

    public void ChangeCharacteristicBy(ValueAndID vaID, int minimum)
    {
        ChangeCharacteristicBy(vaID.ID, vaID.Value, minimum);
    }

    public int GetCharacteristic(int id)
    {
        switch (id)
        {
            case 0:
                return _strength;
            case 1:
                return _endurance;
            case 2:
                return _intelligence;
            case 3:
                return _agility;
            case 4:
                return _luck;
            default:
                Debug.LogWarning("Id of characteristic not found: " + id);
                return -1;
        }
    }

    public void ChangeCharacteristicBy(int id, int amount, int minimum)
    {
        if (amount > _availablePoints)
        {
            Debug.LogWarning("Not enough available points to increase characteristic");
            return;
        }
        if ( !(amount == 1 || amount == -1))
        {
            Debug.LogWarning("Can be changed only by 1 or -1");
            return;
        }
        switch (id)
        {
            case 0:
                _strength = changeCharacteristic(amount, _strength, minimum);
                break;
            case 1:
                _endurance = changeCharacteristic(amount, _endurance, minimum);
                break;
            case 2:
                _intelligence = changeCharacteristic(amount, _intelligence, minimum); 
                break;
            case 3:
                _agility = changeCharacteristic(amount, _agility, minimum);
                break;
            case 4:
                _luck = changeCharacteristic(amount, _luck, minimum);
                break;
            default:
                Debug.LogWarning("Id of characteristic not found: " + id);
                return;
        }

        OnUpdated?.Invoke(this);
    }

    private int changeCharacteristic(int amount, int charactericticValue, int minimum)
    {
        if (charactericticValue == 0 && amount == -1)
            return 0;
        if (amount == -1 && charactericticValue <= minimum)
        {
            Debug.Log("You can not decrease your characteristic more!");
            return charactericticValue;
        }
        if (charactericticValue == MAX_CHARACTERISTIC_VALUE && amount == 1)
            return MAX_CHARACTERISTIC_VALUE;

        _availablePoints -= amount;
        return charactericticValue + amount;
    }

    public void GainExperience(int amount)
    {
        Debug.Log("Experience Gained: " + amount);
        _experience += amount;
        while (_experience >= EXPERIENCE_FOR_LVL[_level])
        {
            if (_level == EXPERIENCE_FOR_LVL.Length - 1)
                return;
            _experience -= EXPERIENCE_FOR_LVL[_level];
            _level++;
            _availablePoints += 2;
        }
    }
}
