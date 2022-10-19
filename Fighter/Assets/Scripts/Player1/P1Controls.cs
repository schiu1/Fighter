using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controls : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator animator;
    CapsuleCollider2D capCollider;
    P1Combat p1combat;
    GameObject p2;

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
    int airDash = 0;

    bool facingRight;
    [HideInInspector]
    public bool p1CanMove; // temp set to true until i implement GameManager
    public bool canCrouch;
    public bool isCrouching;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        capCollider = gameObject.GetComponent<CapsuleCollider2D>();
        p1combat = gameObject.GetComponent<P1Combat>();
        p2 = GameObject.Find("Player2");
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

        p1CanMove = false;
        canCrouch = false;
        isCrouching = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrouching == true)
        {
            p1CanMove = false;
        }

        if (GameManager.gameManager.timedOut == true)
        {
            //prevent walking/jumping/crouching from activating
            p1CanMove = false;
            canCrouch = false;
            
            //make character stand still
            moveHorizontal = 0f;
            moveVertical = 0f;
            animator.SetFloat("Speed", 0);

            //if they are crouching when times out, similar to uncrouch()
            if (isCrouching == true)
            {
                isCrouching = false;
                animator.SetBool("IsCrouching", false);
                capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.54259f);
                capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.27437781f);
            }
        }

        if (p1CanMove)
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
            if (firstPress == 0f && Input.GetButtonDown("P1_Walk") && airDash == 0)
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
        if (canCrouch)
        {
            if (Input.GetButton("P1_Crouch") && isJumping == false && isCrouching == false && p1combat.p1Attacking == false)
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
            if(isJumping == true)
            {
                airDash += 1;
            }
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
        p1CanMove = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
    }

    void startMovement()
    {
        p1CanMove = true;
        rb2D.isKinematic = false;
    }

    void crouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y - 0.54259f); 
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y - 0.27437781f);
        p1CanMove = false;
    }

    void unCrouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.54259f); 
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.27437781f);
        p1CanMove = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("InAir", false);
            if(airDash != 0)
            {
                airDash = 0;
            }
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), p2.GetComponent<CapsuleCollider2D>(), false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
            animator.SetBool("InAir", true);
            //might want to change this somehow in the future to ignore collision only on jump start and not entire jump anim
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), p2.GetComponent<CapsuleCollider2D>(), true);
        }
    }
}
