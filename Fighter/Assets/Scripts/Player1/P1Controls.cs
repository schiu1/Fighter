using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controls : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator animator;

    float speed;
    float maxSpeed;
    float jumpForce;
    public bool isJumping;
    float moveHorizontal;
    float moveVertical;
    bool facingRight;

    [HideInInspector]
    public bool canMove = true; // temp set to true until i implement GameManager

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        speed = 3f;
        maxSpeed = 4f;
        jumpForce = 20f;
        isJumping = false;
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //wrap these two around if notjumping
            moveHorizontal = Input.GetAxisRaw("P1_Horizontal");
            moveVertical = Input.GetAxisRaw("P1_Vertical");
            animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

            if (moveHorizontal > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && facingRight)
            {
                Flip();
            }

            //dash

            //crouch  
        }
    }

    void FixedUpdate()
    {
        if ((moveHorizontal > 0.1f && rb2D.velocity.x < maxSpeed) || (moveHorizontal < -0.1f && rb2D.velocity.x > -maxSpeed))
        {
            rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
        }

        if (moveVertical > 0.1f && !isJumping)
        {
            rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }
}
