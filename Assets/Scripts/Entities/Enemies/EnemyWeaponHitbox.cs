using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    public float weaponDamage = 1f;
    public float knockForce = 600f;
    public Collider2D saberCollider;

    private Vector3 faceUp = new Vector3(-0.59f, 0.99f, 0);
    private Vector3 faceUpSize = new Vector3(2.5f, 0.5f, 0);

    private Vector3 faceDown = new Vector3(-0.67f, -1.53f, 0);
    private Vector3 faceDownSize = new Vector3(2.5f, 0.5f, 0);

    private Vector3 faceLeft = new Vector3(-1.34f, 0.14f, 0);
    private Vector3 faceLeftSize = new Vector3(1f, 1f, 0);

    private Vector3 faceRight = new Vector3(0.96f, 0.16f, 0);
    private Vector3 faceRightSize = new Vector3(1f, 1f, 0);

    private Animator animator;
    private int directionHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (saberCollider == null)
        {
            Debug.LogWarning("Saber collider is not set");
        }
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<Collider2D>().GetComponent<IDamagable>();


        if (damagable != null)
        {
            Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockForce;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            damagable.OnHit(weaponDamage, knockback);
        }
    }

    public void TurnLeft(bool left)
    {

        if (left)
        {
            gameObject.transform.localPosition = faceLeft;
            gameObject.transform.localScale = faceLeftSize;
        }
    }
    public void TurnRight(bool right)
    {

        if (right)
        {
            gameObject.transform.localPosition = faceRight;
            gameObject.transform.localScale = faceRightSize;
        }
    }
    public void TurnUp(bool up)
    {
        if (up)
        {
            gameObject.transform.localPosition = faceUp;
            gameObject.transform.localScale = faceUpSize;
        }
    }
    public void TurnDown(bool down)
    {
        if (down)
        {
            gameObject.transform.localPosition = faceDown;
            gameObject.transform.localScale = faceDownSize;
        }
    }
}
