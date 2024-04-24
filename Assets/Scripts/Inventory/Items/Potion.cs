using UnityEngine;

[CreateAssetMenu]
public class Potion : Item
{
    [SerializeField]
    private int healing;
    public override string Type()
    {
        return "Potion";
    }
    public override int Use()
    {
        Debug.Log("Healing");
        PlayerEvents.GetInstance().Heal(healing);
        return -1;
    }
}
