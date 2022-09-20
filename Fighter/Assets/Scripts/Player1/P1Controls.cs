﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controls : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator animator;

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
    float dashCooldown;
    float direction;

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

        fallMultiplier = 7f;
        isJumping = false;

        facingRight = true;

        firstPress = 0f;
        dash = false;
        dashForce = 30f;
        dashCooldown = 0f;
        direction = 0f;
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
            if(Time.time > firstPress + 0.5f)
            {
                firstPress = 0f;
                direction = 0f;
            }
            if(firstPress == 0f && Input.GetButtonDown("P1_Horizontal"))
            {
                firstPress = Time.time;
                direction = moveHorizontal;
            }
            else if(Time.time < firstPress + 0.5f && firstPress != 0f && direction == moveHorizontal && Input.GetButtonDown("P1_Horizontal"))
            {
                dash = true;
                firstPress = 0f;
                direction = 0f;
            }
            //crouch  
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
            //dash anim
            rb2D.AddForce(new Vector2(moveHorizontal * dashForce, 0f), ForceMode2D.Impulse);
            Debug.Log("dash");
        }

        if (moveVertical > 0.1f && !isJumping)
        {
            //jump anim
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
