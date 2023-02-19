using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgetP1Controls : PlayerControls
{
    BridgetP1Combat p1combat;
    GameObject p2;

    bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        capCollider = gameObject.GetComponent<CapsuleCollider2D>();
        p1combat = gameObject.GetComponent<BridgetP1Combat>();
        p2 = GameObject.Find(GameManager.gameManager.p2Name);
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

        canMove = false;
        canCrouch = false;
        isCrouching = false;

        pushback = false;
        pushForceX = 0f;
        pushForceY = 0f;

        winAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused)
        {
            if(isCrouching == true)
            {
                canMove = false;
            }

            //when times out or gets kill
            if(GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0)
            {
                //prevent walking/jumping/crouching
                canMove = false;
                canCrouch = false;

                //make character stand still
                moveHorizontal = 0f;
                moveVertical = false;
                animator.SetFloat("Speed", 0);

                //if they are crouching when times out, similar to uncrouch
                if(isCrouching == true)
                {
                    isCrouching = false;
                    animator.SetBool("IsCrouching", false);
                    capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.872712f);
                    capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.43635579f);
                }

                //do win or lose anim
                if((GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health) && !winAnim)
                {
                    StartCoroutine(WinAnimation());
                    winAnim = true;
                }
                else if ((GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health))
                {
                    animator.SetBool("Lost", true);
                }
            }

            if (canMove)
            {
                
                moveHorizontal = Input.GetAxisRaw("P1_Walk");
                if(Input.GetButtonDown("P1_Jump") && !isJumping)
                {
                    Debug.Log("jump");
                    moveVertical = true;
                }

                animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

                if(moveHorizontal > 0 && !facingRight)
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
            {                                                                                    //this is why its not in base class
                if(Input.GetButton("P1_Crouch") && isJumping == false && isCrouching == false && p1combat.attacking == false)
                {
                    animator.SetTrigger("Crouch");
                    animator.SetBool("IsCrouching", true);
                    isCrouching = true;
                    Crouch();
                    StopMovement();
                }
                else if(!Input.GetButton("P1_Crouch") && isCrouching == true)
                {
                    animator.SetBool("IsCrouching", false);
                    StartMovement();
                    Uncrouch();
                    isCrouching = false;
                }
            }
        }
    }

    void WinAnimFaceRight()
    {
        if (facingRight)
        {
            Flip();
        }
    }

    public override void Pushback(string pushType)
    {
        if (pushType == "flinch")
        {
            animator.SetTrigger("Flinch");
        }
        else if (pushType == "push")
        {
            canMove = false;
            canCrouch = false;
            moveHorizontal = 0;
            p1combat.canAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("Push");
            if (gameObject.transform.position.x - p2.transform.position.x > 0)
            {
                if (facingRight) { Flip(); }
                pushForceX = 15f;
                Debug.Log("pushing right");
            }
            else if (gameObject.transform.position.x - p2.transform.position.x < 0)
            {
                if (!facingRight) { Flip(); }
                pushForceX = -15f;
                Debug.Log("pushing left");
            }
            pushback = true;
        }
        else if (pushType == "knockdown")
        {
            canMove = false;
            canCrouch = false;
            moveHorizontal = 0;
            p1combat.canAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("KDAir");
            if (gameObject.transform.position.x - p2.transform.position.x > 0)
            {
                if (facingRight) { Flip(); }
                pushForceX = 15f;
                Debug.Log("KD right");
            }
            else if (gameObject.transform.position.x - p2.transform.position.x < 0)
            {
                if (!facingRight) { Flip(); }
                pushForceX = -15f;
                Debug.Log("KD left");
            }
            pushForceY = 5f;
            pushback = true;
            KDGround = true;
        }
    }

    void PushEnd()
    {
        canMove = true;
        canCrouch = true;
        rb2D.isKinematic = false; 
        p1combat.canAttack = true;
    }

    public override void BlockAttack()
    {
        animator.SetTrigger("Block");
        if (gameObject.transform.position.x - p2.transform.position.x > 0)
        {
            if (facingRight) { Flip(); }
        }
        else if (gameObject.transform.position.x - p2.transform.position.x < 0)
        {
            if (!facingRight) { Flip(); }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }

    void StopMovement()
    {
        canMove = false;
        //canCrouch = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        moveHorizontal = 0f;
        animator.SetFloat("Speed", 0);
    }

    void StartMovement()
    {
        canMove = true;
        //canCrouch = true;
        rb2D.isKinematic = false;
    }

    void Crouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y - 0.872712f);
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y - 0.43635579f);
        canMove = false;
    }

    void Uncrouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.872712f);
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.43635579f);
        canMove = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("InAir", false);
            if (airDash != 0)
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
