using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GoblinController : MonoBehaviour
{
    public float damage = 1;
    public float knockForce = 15f;
    public DetectionRange range;
    public float moveSpeed = 500f;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (range.detectedObjects.Count > 0)
        {
            //Calculate direction to the target
            Vector2 direction = (range.detectedObjects[0].transform.position - transform.position).normalized;

            //Move towards player
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        } else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable damagable = collision.collider.GetComponent<IDamagable>();

        if(damagable != null )
        {
            Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockForce;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            Attack();
            damagable.OnHit(damage, knockback);
        }
    }

    private void Attack()
    {
        animator.SetTrigger("GoblinAttack");
    }
}
