using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public float weaponDamage = 1f;

    public Collider2D swordCollider;

    private Vector3 faceUp = new Vector3(-0.59f, 0.99f, 0);
    private Vector3 faceUpSize = new Vector3(2.5f, 0.5f, 0);

    private Vector3 faceDown = new Vector3(-0.67f, -1.53f, 0);
    private Vector3 faceDownSize = new Vector3(2.5f, 0.5f, 0);

    private Vector3 faceLeft = new Vector3(-1.34f, 0.14f, 0);
    private Vector3 faceLeftSize = new Vector3(1f, 1f, 0);

    private Vector3 faceRight = new Vector3(0.96f, 0.16f, 0);
    private Vector3 faceRightSize = new Vector3(1f, 1f, 0);

    private int directionHit = 0;

    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private PlayerCharacteristics characteristics;
    private void Start()
    {
        if(swordCollider == null)
        {
            Debug.LogWarning("Sword collider is not set");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable =  collision.GetComponent<IDamagable>();

        if(damagable != null)
        {
            Weapon.Damage damage = inventory.GetDamage();

            Debug.Log("Damage: " + damage.Amount);

            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2) (collision.gameObject.transform.position - parentPos).normalized;
            Vector2 knockback = direction * damage.Knock;

            damagable.OnHit(damage.Amount * (1 + 0.1f * characteristics.Strength), knockback * (1 + 0.05f * characteristics.Strength), directionHit);
        } 
    }

    public void TurnLeft(bool left)
    {
        
        if (left)
        {
            gameObject.transform.localPosition = faceLeft;
            gameObject.transform.localScale = faceLeftSize;
            directionHit = 1;
        } 
    }
    public void TurnRight(bool right)
    {
        if (right)
        {
            gameObject.transform.localPosition = faceRight;
            gameObject.transform.localScale = faceRightSize;
            directionHit = 0;
        }
    }
    public void TurnUp(bool up)
    {
        if (up)
        {
            gameObject.transform.localPosition = faceUp;
            gameObject.transform.localScale = faceUpSize;
            directionHit = 2;
        }
    }
    public void TurnDown(bool down)
    {
        if (down)
        {
            gameObject.transform.localPosition = faceDown;
            gameObject.transform.localScale = faceDownSize;
            directionHit = 3;
        }
    }
}
