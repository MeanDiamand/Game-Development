using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

[CreateAssetMenu]
public class Weapon : Item
{
    [field: SerializeField]
    public Damage damage { get; set; }
    public Weapon() 
    {}
    public Weapon(Damage damage) 
    {
        this.damage = damage;
    }
    
    public override Damage dealDamage()
    {
        Debug.Log("Weapon");
        return damage;
    }

    public override string Type()
    {
        return "Weapon";
    }

    [System.Serializable]
    public class Damage
    {
        public float Amount { get; set; }
        public float Knock { get; set; }
        public DamageTypes DamageType { get; set; }
        public static Damage HandDamage()
            => new Damage
            {
                Amount = 1f,
                Knock = 15,
                DamageType = DamageTypes.Physical
            };
    }
 
}