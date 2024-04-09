using Assets.Interfaces;
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
        [SerializeField]
        public List<ItemPUS> deathItemPrefabs;

        private Animator animator;
        private Rigidbody2D rb;
        private Collider2D physicsCollider;
        private FloatingStatusBar statusBar;
        private bool isAlive = true;
        private float counter = 0;
        private int EXP_FOR_KILL = 10;

        private float maxHealth = 5;

        private float _health = 5;
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

                if (_health > maxHealth)
                    _health = maxHealth;

                if (_health <= 0)
                {
                    animator.SetBool("isAlive", false);
                    rb.velocity = Vector2.zero;
                    rb.isKinematic = true;
                    IsHitable = false;
                    if (counter >= 4.0)
                    {
                        GameOverEvents.isGameOver = true;
                    }
                }
            }
        }

        private bool _isHitable = true;
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

        public void Start()
        {
            statusBar = GetComponentInChildren<FloatingStatusBar>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            physicsCollider = GetComponent<Collider2D>();
            animator.SetBool("isAlive", true);

            if (!isSimulated)
            {
                PlayerEvents.GetInstance().OnHealed += Heal;
            }

            PlayerEvents.GetInstance().OnGameStart += GameStarted;
        }

        public void Heal(int amount)
        {
            Health += amount;
        }

        public void OnHit(float damage, Vector2 knockDirection, int direction)
        {
            Health -= damage;
            statusBar.UpdateStatusBar(Health, maxHealth);
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
            counter += damage;
            rb.AddForce(knockDirection, ForceMode2D.Impulse);
        }

        public void OnHit(float damage)
        {
            Health -= damage;
            counter += damage;
        }

        private void GameStarted()
        {
            Health = maxHealth;
        }

        public void KillObject()
        {
            Instantiate(PickDeathPrefab(), transform.position, Quaternion.identity);
            PlayerEvents.GetInstance().ExperienceGained(EXP_FOR_KILL);
            Destroy(gameObject);
        }

        private ItemPUS PickDeathPrefab()
        {
            if (deathItemPrefabs.Count == 0)
                return null;
            System.Random random = new System.Random();
            return deathItemPrefabs[random.Next(deathItemPrefabs.Count)];
        }

    }
}
