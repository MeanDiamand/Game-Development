using Assets.Interfaces;
using UnityEngine;

public class WeaponHitbox : ObjectHitbox
{
    [SerializeField]
    private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            Damage damage = playerController.GetDamage();
            //Debug.Log("Damage: " + damage.Amount);

            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;
            Vector2 knockback = direction * damage.Knock;

            damagable.OnHit(damage.Amount, knockback, 0);
        }
    }
}
