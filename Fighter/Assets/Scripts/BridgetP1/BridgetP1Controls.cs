﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgetP1Controls : PlayerControls
{
    BridgetP1Combat p1combat;
    GameObject p2;

    bool facingRight;
    [HideInInspector]
    public bool p1CanMove; //maybe its fine staying like this?

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

        p1CanMove = false;
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
                p1CanMove = false;
            }

            //when times out or gets kill
            if(GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0)
            {
                //prevent walking/jumping/crouching
                p1CanMove = false;
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
                    //make capCollider size normal here
                    //make offset normal here
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

            if (p1CanMove)
            {
                moveHorizontal = Input.GetAxisRaw("P1_Walk");
                if(Input.GetButtonDown("P1_Jump") && !isJumping)
                {
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
                if(Input.GetButton("P1_Crouch") && isJumping == false && isCrouching == false && p1combat.p1Attacking == false)
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

    public void Pushback(string pushType)
    {
        if (pushType == "flinch")
        {
            animator.SetTrigger("Flinch");
        }
        else if (pushType == "push")
        {
            p1CanMove = false;
            moveHorizontal = 0;
            p1combat.p1CanAttack = false;
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
            p1CanMove = false;
            moveHorizontal = 0;
            p1combat.p1CanAttack = false;
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
        p1CanMove = true;
        rb2D.isKinematic = false; 
        p1combat.p1CanAttack = true;
    }

    public void BlockAttack()
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
        p1CanMove = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        moveHorizontal = 0f;
        animator.SetFloat("Speed", 0);
    }

    void StartMovement()
    {
        p1CanMove = true;
        rb2D.isKinematic = false;
    }

    void Crouch()
    {
        //shorten height of capsule collider to whatever height the sprite is
        //move offset of capsule collider down 1/2 of the height difference
        p1CanMove = false;
    }

    void Uncrouch()
    {
        //increase height by same amount it was decreased when crouching
        //move offset up by same amount it was moved down when crouching
        p1CanMove = true;
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
