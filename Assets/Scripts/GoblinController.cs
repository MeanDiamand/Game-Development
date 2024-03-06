using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour, IDamagable
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private bool isAlive = true;
    public float Health
    {
        get { return _health; }
        set
        {
            if (value < _health)
            {
                animator.SetTrigger("Hit");
            }

            _health = value;

            if (_health <= 0)
            {
                animator.SetBool("isAlive", false);
                IsHitable = false;
            }
        }
    }
    public bool IsHitable { 
        get { return _isHitable; }
        set 
        { 
            _isHitable = value;
            rb.simulated = value;
            physicsCollider.enabled = value;
        } 
    }


    public float _health = 3;

    public bool _isHitable = true;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        animator.SetBool("isAlive", true);
    }

    public void OnHit(float damage, Vector2 knockDirection)
    {
        Health -= damage;
        rb.AddForce(knockDirection);
    }

    public void OnHit(float damage)
    {
        Debug.Log("Goblin hit for " + damage);
        Health -= damage;
    }

    public void KillObject()
    {
        Destroy(gameObject);
    }
}
