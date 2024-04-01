using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

// NOTE: The movement for this script uses the new InputSystem. The player needs to have a PlayerInput
// component added and the Behaviour should be set to Send Messages so that the OnMove and OnFire methods
// actually trigger

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 600f;
    public float maxSpeed = 10f;
    public float collisionOffset = 0.05f;
    public float cooldown = 1;
    public ContactFilter2D contactFilter;
    public float attackRange = 0.5f;
    public float idleFriction = 0.9f;
    public GameObject attackPoint;

    private Vector2 input;
    private Animator animator;
    private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    private Collider2D swordCollider;
    private float lastHit;

    private bool isMoving = false;
    private bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    [SerializeField]
    private PlayerCharacteristics characteristics;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swordCollider = attackPoint.GetComponent<Collider2D>();
    }

    private void Awake()
    {
        //start the animation by get the component animator from the player
        animator = GetComponent<Animator>();

    }

    public void FixedUpdate()
    {
        MouseLook();
        if (input != Vector2.zero)
        {
            Move(input);
        } else
        {
            Stop();
        }
    }

    // Tries to move the player in a direction by casting in that direction by the amount
    // moved plus an offset. If no collisions are found, it moves the players
    // Returns true or false depending on if a move was executed
    public void Move(Vector2 direction)
    {
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity + (direction * moveSpeed * Time.deltaTime), maxSpeed);
        rb.AddForce(input * moveSpeed * (1 + 0.1f * characteristics.Agility) * Time.deltaTime);
        if(rb.velocity.magnitude > maxSpeed)
        {
            float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
            rb.velocity = rb.velocity.normalized * limitedSpeed;
        }
        IsMoving = true;
    }

    public void Stop()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
        IsMoving = false;
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    private void OnFire()
    {
        SwordAttack();
    }

    private void SwordAttack()
    {
        if(Time.time - lastHit < cooldown)
        {
            return;
        }
        lastHit = Time.time;
        animator.SetTrigger("SwordAttack");
    }

    private void MouseLook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        checkHitboxDirection(direction.x, direction.y);

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