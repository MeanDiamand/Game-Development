using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Weapon : WearableItem
{
    [field: SerializeField]
    [JsonProperty]
    public Damage damage { get; private set; }
    
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
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [CreateAssetMenu]
    public class Damage
    {
        [JsonProperty]
        public float Amount { get; set; }

        [JsonProperty]
        public float Knock { get; set; }

        [JsonProperty]
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