using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    private Animator animator;
    private bool isAlive = true;
    private float Health
    {
        set
        {
            if (value < _health)
            {
                animator.SetTrigger("Hit");
            }

            _health = value;

            if(_health <= 0) {
                animator.SetBool("isAlive", false);
            }
        }
        get { return _health; }
    }
    public float _health = 3;

    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", true);
    }

    private void OnHit(float damage)
    {
        Debug.Log("Goblin hit for " + damage);
        Health -= damage;
    }
}
