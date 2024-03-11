using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public float weaponDamage = 1f;

    public Collider2D swordCollider;

    public Vector3 faceUp = new Vector3(-0.59f, 0.99f, 0);
    public Vector3 faceDown = new Vector3(-0.67f, -1.53f, 0);
    public Vector3 faceLeft = new Vector3(-1.34f, 0.14f, 0);
    public Vector3 faceRight = new Vector3(0.96f, 0.16f, 0);

    public float knockForce = 15f;

    private int directionHit = 0;

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
            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2) (collision.gameObject.transform.position - parentPos).normalized;
            Vector2 knockback = direction * knockForce;

            damagable.OnHit(weaponDamage, knockback, directionHit);
        } else
        {
            Debug.LogWarning("Collider does not implement IDamagable");
        }
    }

    public void TurnLeft(bool left)
    {
        if (left)
        {
            gameObject.transform.localPosition = faceLeft;
            directionHit = 1;
        } 
    }
    public void TurnRight(bool right)
    {
        if (right)
        {
            gameObject.transform.localPosition = faceRight;
            directionHit = 0;
        }
    }
    public void TurnUp(bool up)
    {
        if (up)
        {
            gameObject.transform.localPosition = faceUp;
            directionHit = 2;
        }
    }
    public void TurnDown(bool down)
    {
        if (down)
        {
            gameObject.transform.localPosition = faceDown;
            directionHit = 3;
        }
    }
}
