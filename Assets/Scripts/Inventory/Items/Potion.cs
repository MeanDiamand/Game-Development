using UnityEngine;

[CreateAssetMenu]
public class Potion : Item
{
    public int Healing { get; set; }
    public Potion() { }
    public override string Type()
    {
        return "Potion";
    }
    public override void Use()
    {
        Debug.Log("Healing");
    }
}
