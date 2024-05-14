using Assets.Interfaces;
using UnityEngine;

public class EnemyWeaponHitbox : ObjectHitbox
{
    public float knockForce = 600f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
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
}
