using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controls : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator animator;
    CapsuleCollider2D capCollider;

    float speed;
    float maxSpeed;
    float moveHorizontal;
    float moveVertical;

    float jumpForce;
    float fallMultiplier;
    public bool isJumping;

    float firstPress;
    bool dash;
    float dashForce;
    float direction;

    bool facingRight;
    [HideInInspector]
    public bool canMove = true; // temp set to true until i implement GameManager
    public bool canCrouch = true;
    public bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        capCollider = gameObject.GetComponent<CapsuleCollider2D>();
        speed = 3f;
        maxSpeed = 4f;
        jumpForce = 20f;

        fallMultiplier = 7f;
        isJumping = false;

        facingRight = true;

        firstPress = 0f;
        dash = false;
        dashForce = 30f;
        direction = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrouching == true)
        {
            canMove = false;
        }

        if (canMove)
        {
            //wrap these two around if notjumping
            moveHorizontal = Input.GetAxisRaw("P1_Walk");
            moveVertical = Input.GetAxisRaw("P1_Jump");
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
            if(Time.time > firstPress + 0.5f || direction == -moveHorizontal)
            {
                firstPress = 0f;
                direction = 0f;
            }
            if(firstPress == 0f && Input.GetButtonDown("P1_Walk"))
            {
                firstPress = Time.time;
                direction = moveHorizontal;
            }
            else if(Time.time < firstPress + 0.5f && firstPress != 0f && direction == moveHorizontal && Input.GetButtonDown("P1_Walk"))
            {
                dash = true;
                firstPress = 0f;
                direction = 0f;
            }
        }
        if (canCrouch) //issue with player still moving when pressing crouch while walking
        {
            if (Input.GetButton("P1_Crouch") && isJumping == false && isCrouching == false)
            {
                animator.SetTrigger("Crouch");
                animator.SetBool("IsCrouching", true);
                isCrouching = true;
                crouch();
                stopMovement();
            }
            else if(Input.GetButtonUp("P1_Crouch") && isCrouching == true)
            {
                animator.SetBool("IsCrouching", false);
                startMovement();
                unCrouch();
                isCrouching = false;
            }

        }
    }

    void FixedUpdate()
    {
        if ((moveHorizontal > 0.1f && rb2D.velocity.x < maxSpeed) || (moveHorizontal < -0.1f && rb2D.velocity.x > -maxSpeed))
        {
            rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
        }

        if(dash == true)
        {
            dash = false;
            animator.SetTrigger("Dash");
            rb2D.AddForce(new Vector2(moveHorizontal * dashForce, 0f), ForceMode2D.Impulse);
            Debug.Log("dash");
        }

        if (moveVertical > 0.1f && !isJumping)
        {
            animator.SetTrigger("Jump");
            rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        }
        //falling helper
        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }

    void stopMovement()
    {
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
    }

    void startMovement()
    {
        rb2D.isKinematic = false;
    }

    void crouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y - 0.54259f); 
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y - 0.27437781f);
        canMove = false;
        Debug.Log("crouch");
    }

    void unCrouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.54259f); 
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.27437781f);
        canMove = true;
        Debug.Log("unCrouch");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("InAir", false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
            animator.SetBool("InAir", true);
        }
    }
}
