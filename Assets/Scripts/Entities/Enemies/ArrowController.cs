using Assets.Interfaces;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed;
    public float arrowDamage = 1f;
    public float knockForce = 200f;

    private string tagTarget = "Player";
    private GameObject target;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag(tagTarget);

        Vector2 direction = (target.transform.position - transform.position).normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
        rb.velocity = new Vector2 (direction.x, direction.y);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<Collider2D>().GetComponent<IDamagable>();
        if (damagable != null)
        {
            Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockForce;

            damagable.OnHit(arrowDamage, knockback);
            Destroy(this.gameObject);
        }
    }
}
