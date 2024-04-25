using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

[CreateAssetMenu]
public class Weapon : WearableItem
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
        public Damage NewAdded(float factor, DamageTypes newDamageType)
        {
            Damage newDamage = new Damage { Amount = this.Amount, Knock = this.Knock, DamageType = this.DamageType };
            if (newDamageType == DamageType)
                newDamage.Amount += factor;
            return newDamage;
        }

        public Damage NewMultiplied(float factor, DamageTypes newDamageType)
        {
            Damage newDamage = new Damage { Amount = this.Amount, Knock = this.Knock, DamageType = this.DamageType };
            if (newDamageType == DamageType)
                newDamage.Amount *= factor;
            return newDamage;
        }
    }
}