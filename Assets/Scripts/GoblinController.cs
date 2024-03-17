using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GoblinController : MonoBehaviour
{
    public DetectionRange detectionRrange;
    public AttackRange attackRrange;
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
        if (detectionRrange.detectedObjects.Count > 0)
        {
            //Calculate direction to the target
            Vector2 direction = (detectionRrange.detectedObjects[0].transform.position - transform.position).normalized;

            //Move towards player
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            if (attackRrange.detectedObjects.Count > 0)
            {
                Attack();
            }
            checkHitboxDirection(direction.x, direction.y);
        } else
        {
            animator.SetBool("isMoving", false);
        }
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    IDamagable damagable = collision.collider.GetComponent<IDamagable>();

    //    if(damagable != null )
    //    {
    //        Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
    //        Vector2 knockback = direction * knockForce;
    //        animator.SetFloat("moveX", direction.x);
    //        animator.SetFloat("moveY", direction.y);

    //        damagable.OnHit(damage, knockback);
    //    }
    //}

    private void Attack()
    {
        animator.SetTrigger("GoblinAttack");
    }

    private void checkHitboxDirection(float x, float y)
    {
        bool right = x > 0 && (y < 1.5 && y > -1.5);
        bool left = x < 0 && (y < 1.5 && y > -1.5);
        bool up = y > 0.5;
        bool down = y < -0.5;

        if (right)
        {
            gameObject.BroadcastMessage("TurnRight", right);
        }
        else if (left)
        {
            gameObject.BroadcastMessage("TurnLeft", left);
        }
        if (up)
        {
            gameObject.BroadcastMessage("TurnUp", up);
        }
        else if (down)
        {
            gameObject.BroadcastMessage("TurnDown", down);
        }
    }
}
