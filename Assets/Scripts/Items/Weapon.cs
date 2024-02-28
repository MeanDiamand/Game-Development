using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public int Damage { get; set; }
    public DamageTypes DamageType { get; set; }
    public Weapon() { }
}