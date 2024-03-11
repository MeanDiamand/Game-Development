﻿using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class DamagableCharacter: MonoBehaviour, IDamagable
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
        public bool IsHitable
        {
            get { return _isHitable; }
            set
            {
                _isHitable = value;
                if(!isSimulated)
                {
                    rb.simulated = false;
                }
                physicsCollider.enabled = value;
            }
        }

        public bool isSimulated = true;
        public float _health = 5;
        public bool _isHitable = true;

        public void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            physicsCollider = GetComponent<Collider2D>();
            animator.SetBool("isAlive", true);
        }

        public void OnHit(float damage, Vector2 knockDirection, int direction)
        {
            Health -= damage;
            switch (direction)
            {
                //Hit from right
                case 0:
                    animator.SetFloat("moveX", -1);
                    animator.SetFloat("moveY", 0);
                    break;
                //Hit from left
                case 1:
                    animator.SetFloat("moveX", 1);
                    animator.SetFloat("moveY", 0);
                    break;
                //Hit from above
                case 2:
                    animator.SetFloat("moveX", 0);
                    animator.SetFloat("moveY", -1);
                    break;
                //Hit from below
                case 3:
                    animator.SetFloat("moveX", 0);
                    animator.SetFloat("moveY", 1);
                    break;
            }
            rb.AddForce(knockDirection, ForceMode2D.Impulse);
        }

        public void OnHit(float damage, Vector2 knockDirection)
        {
            Health -= damage;
            rb.AddForce(knockDirection, ForceMode2D.Impulse);
        }

            public void OnHit(float damage)
        {
            Health -= damage;
        }

        public void KillObject()
        {
            Destroy(gameObject);
        }

    }
}
