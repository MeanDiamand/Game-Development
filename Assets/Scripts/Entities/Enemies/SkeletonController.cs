using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public DetectionRange detectionRange;
    public DetectionRange attackRrange;
    public DetectionRange retreatRange;
    public GameObject arrow;
    public GameObject arrowParent;

    [SerializeField]
    private float moveSpeed = 500f;
    [SerializeField]
    private float startShootCooldown;

    private float shootCooldown;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject arrowPosition;

    private Vector3 faceUp = new Vector3(-0.18f, 1.93f, 0);
    private Vector3 faceDown = new Vector3(-0.12f, -2.16f, 0);
    private Vector3 faceLeft = new Vector3(-3.15f, 0.24f, 0);
    private Vector3 faceRight = new Vector3(1.96f, 0.24f, 0);

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shootCooldown = startShootCooldown;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        arrowPosition = GameObject.FindGameObjectWithTag("ArrowPosition");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (detectionRange.detectedObjects.Count > 0 || attackRrange.detectedObjects.Count < 0 && retreatRange.detectedObjects.Count < 0)
        {
            Vector2 initialVelocity = rb.velocity;
            moveSpeed = 2000;
            Vector2 direction = (detectionRange.detectedObjects[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            rb.isKinematic = false;

            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            checkHitboxDirection(direction.x, direction.y);

            //Check if ennemy is close enough to a player to shoot
            if (attackRrange.detectedObjects.Count > 0 || retreatRange.detectedObjects.Count < 0)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                animator.SetBool("isMoving", false);
                animator.SetFloat("moveX", (float)System.Math.Round(direction.x));
                animator.SetFloat("moveY", (float)System.Math.Round(direction.y));
                StartShootAnimation();


            }
            // Check if player is too close to an enemy to retreat
            if(retreatRange.detectedObjects.Count > 0)
            {
                rb.isKinematic = false;
                moveSpeed = 800;
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

    private void StartShootAnimation()
    {
        if (shootCooldown <= 0)
        {
            audioManager.PlayEffect(audioManager.BowShot);
            animator.SetTrigger("Shoot");
            shootCooldown = startShootCooldown;
        } else
        {
            shootCooldown -= Time.deltaTime; 
        }
    }

    public void ShootArrow()
    {
        Debug.Log("ShootArrow method called.");
        Instantiate(arrow, arrowParent.transform.position, Quaternion.identity);
    }

    private void checkHitboxDirection(float x, float y)
    {
        bool right = x > 0 && (y < 1.5 && y > -1.5);
        bool left = x < 0 && (y < 1.5 && y > -1.5);
        bool up = y > 0.5;
        bool down = y < -0.5;

        if (right)
        {
            arrowPosition.transform.localPosition = faceRight;
        }
        else if (left)
        {
            arrowPosition.transform.localPosition = faceLeft;
        }
        if (up)
        {
            arrowPosition.transform.localPosition = faceUp;
        }
        else if (down)
        {
            arrowPosition.transform.localPosition = faceDown;
        }
    }
}
