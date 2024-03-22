using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int ID => GetInstanceID();

    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    public float Weight { get; set; }
    [field: SerializeField]
    public bool IsStackable { get; set; }
    [field: SerializeField]
    public int MaxStackSize { get; set; } = 1;

    [field: SerializeField]
    [field: TextArea]
    public string Description { get; set; }

    [field: SerializeField]
    public Sprite ItemIcon { get; set; }
    public override string ToString()
    {
        return Name;
    }
    public virtual Weapon.Damage dealDamage()
    {
        return null;
    }
    public virtual void Use()
    {
        Debug.LogError("Use() not implemented in item");
        throw new Exception("Use() not implemented in item");
    }
    public virtual string Type()
    {
        return "Item";
    }
}
