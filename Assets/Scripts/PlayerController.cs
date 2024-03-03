using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;

    private Vector2 input;
    private Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;

    //specify the layer use with physic
    public LayerMask solidObjectsLayer;

    //called with the script is being loaded
    private void Awake()
    {
        //start the animation by get the component animator from the player
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseLook();

        if(Input.GetMouseButtonDown(0))
        {
            SwordAttack();
        }

        if (!isMoving)
        {
            //GetAxisRaw() is function of UnityEngine library for getting the raw input from the user.
            input.x = Input.GetAxisRaw("Horizontal"); 
            input.y = Input.GetAxisRaw("Vertical");


            if(input.x != 0) //if moving to left/right, then the character will not move up/down
            {
                input.y = 0;
            }

            if(input!=Vector2.zero)
            {
                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                if (IsWalkable(targetPosition))
                {
                    StartCoroutine(Move(targetPosition)); //running constantly in the game
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
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


    private void SwordAttack()
    {
        animator.SetTrigger("SwordAttack");
        
        

        foreach (Collider2D enemy in Physics2D.OverlapCircleAll(attackPoint.position, attackRange))
        {
            Debug.Log("We hit " + enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;
        while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {                                           //current position     destination           movement      
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPosition) 
    {
        if(Physics2D.OverlapCircle(targetPosition, 0.6f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
