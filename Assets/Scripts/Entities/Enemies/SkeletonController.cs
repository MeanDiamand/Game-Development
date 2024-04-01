using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public DetectionRange detectionRange;
    public DetectionRange attackRrange;
    public DetectionRange retreatRange;
    public GameObject arrow;
    public float moveSpeed = 500f;
    public float startShootCooldown;

    private float shootCooldown = 1f;
    private Rigidbody2D rb;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        shootCooldown = startShootCooldown;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (detectionRange.detectedObjects.Count > 0 || attackRrange.detectedObjects.Count < 0 && retreatRange.detectedObjects.Count < 0)
        {
            Vector2 initialVelocity = rb.velocity;
            Vector2 direction = (detectionRange.detectedObjects[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            rb.isKinematic = false;

            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            //Check if ennemy is close enough to a player to shoot
            if (attackRrange.detectedObjects.Count > 0 || retreatRange.detectedObjects.Count < 0)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                animator.SetBool("isMoving", false);
                animator.SetFloat("moveX", direction.x);
                animator.SetFloat("moveY", direction.y);
                Fire();


            }
            // Check if player is too close to an enemy to retreat
            if(retreatRange.detectedObjects.Count > 0)
            {
                rb.isKinematic = false;
                rb.velocity = initialVelocity;
                rb.AddForce(-direction * moveSpeed * Time.deltaTime);
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", -direction.x);
                animator.SetFloat("moveY", -direction.y);
            }

        } else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void Fire()
    {
        if (shootCooldown <= 0)
        {
            Instantiate(arrow, transform.position, transform.rotation);
            shootCooldown = startShootCooldown;
        } else
        {
            shootCooldown -= Time.deltaTime; 
        }
    }
}
