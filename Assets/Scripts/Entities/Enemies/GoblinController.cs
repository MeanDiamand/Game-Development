using Assets.Scripts;
using UnityEngine;

public class GoblinController : DamagableCharacter
{
    public DetectionRange detectionRrange;
    public DetectionRange attackRrange;

    [SerializeField]
    private float moveSpeed = 500f;
    [SerializeField]
    private float cooldown = 1;

    private Rigidbody2D rb;
    private Animator animator;
    private float lastHit;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (detectionRrange.detectedObjects.Count > 0)
        {
            //Calculate direction to the target
            Vector2 direction = (detectionRrange.detectedObjects[0].transform.position - transform.position).normalized;

            //Move towards player
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            if (attackRrange.detectedObjects.Count > 0)
            {
                Attack();
            }
            checkHitboxDirection(direction.x, direction.y);
        } else
        {
            animator.SetBool("isMoving", false);
        }
        
    }

    private void Attack()
    {
        if (Time.time - lastHit < cooldown)
        {
            return;
        }
        lastHit = Time.time;
        audioManager.PlayEffect(audioManager.hitting);
        animator.SetTrigger("GoblinAttack");
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
