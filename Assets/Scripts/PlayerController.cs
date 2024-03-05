using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// NOTE: The movement for this script uses the new InputSystem. The player needs to have a PlayerInput
// component added and the Behaviour should be set to Send Messages so that the OnMove and OnFire methods
// actually trigger

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D contactFilter;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    private Vector2 input;
    private Animator animator;
    private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    private bool isMoving;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        //start the animation by get the component animator from the player
        animator = GetComponent<Animator>();

    }

    public void FixedUpdate()
    {

        MouseLook();

        // Try to move player in input direction, followed by left right and up down input if failed
        bool hasMoved = Move(input);
        
        if (!hasMoved)
        {
            // Try Left / Right
            hasMoved = Move(new Vector2(input.x, 0));

            if (!hasMoved)
            {
                hasMoved = Move(new Vector2(0, input.y));
            }
        }
        

    }

    // Tries to move the player in a direction by casting in that direction by the amount
    // moved plus an offset. If no collisions are found, it moves the players
    // Returns true or false depending on if a move was executed
    public bool Move(Vector2 direction)
    {
        // Check for potential collisions
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            contactFilter, // The settings that determine where a collision can occur on such as layers to collide with
            collisions, // List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

        if (count == 0)
        {
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            // No collisions
            rb.MovePosition(rb.position + moveVector);
            
            return true;
        }
        else
        {
            // Print collisions
            foreach (RaycastHit2D item in collisions)
            {
                print(item.ToString());
            }

            return false;
        }
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void SwordAttack()
    {
        animator.SetTrigger("SwordAttack");



        foreach (Collider2D enemy in Physics2D.OverlapCircleAll(attackPoint.position, attackRange))
        {
            Debug.Log("We hit " + enemy.name);
        }
    }

    private void MouseLook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        // Calculate a position offset from the character's position in the direction of the mouse
        Vector2 attackPointOffset = direction.normalized * 2f; // Adjust the multiplier to set the distance from the character
        attackPoint.position = (Vector2)transform.position + attackPointOffset;
    }
}