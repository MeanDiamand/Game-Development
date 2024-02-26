using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    //called with the script is being loaded
    private void Awake()
    {
        //start the animation by get the component animator from the player
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isMoving)
        {
            //GetAxisRaw() is function of UnityEngine library for getting the raw input from the user.
            input.x = Input.GetAxisRaw("Horizontal"); 
            input.y = Input.GetAxisRaw("Vertical");

            Debug.Log("This is input.x" + input.x);
            Debug.Log("This is input.y" + input.y);

            if(input.x != 0) //if moving to left/right, then the character will not move up/down
            {
                input.y = 0;
            }

            if(input!=Vector2.zero)
            {
                //make the moveX/moveY input equal to that input for X/Y
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                StartCoroutine(Move(targetPosition)); //running constantly in the game
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;
        while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {                                            //current position   destination           movement      
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        isMoving = false;
    }
}
