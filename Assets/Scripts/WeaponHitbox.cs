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

    private void Start()
    {
        if(swordCollider == null)
        {
            Debug.LogWarning("Sword collider is not set");
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("HIT");
    //    collision.collider.SendMessage("OnHit", weaponDamage);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT");
       collision.SendMessage("OnHit", weaponDamage);
    }

    public void TurnLeft(bool left)
    {
        if (left)
        {
            gameObject.transform.localPosition = faceLeft;
        } 
    }
    public void TurnRight(bool right)
    {
        if (right)
        {
            gameObject.transform.localPosition = faceRight;
        }
    }
    public void TurnUp(bool up)
    {
        if (up)
        {
            gameObject.transform.localPosition = faceUp;
        }
    }
    public void TurnDown(bool down)
    {
        if (down)
        {
            gameObject.transform.localPosition = faceDown;
        }
    }
}
