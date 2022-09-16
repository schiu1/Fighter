using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controls : MonoBehaviour
{
    Rigidbody2D rb2D;

    float speed;
    float maxSpeed;
    float jumpForce;
    bool isJumping;
    float moveHorizontal;
    float moveVertical;

    [HideInInspector]
    public bool canMove = true; // temp set to true until i implement GameManager

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        speed = 4f;
        maxSpeed = 5f;
        jumpForce = 20f;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //wrap these two around if notjumping
            moveHorizontal = Input.GetAxisRaw("P1_Horizontal");
            moveVertical = Input.GetAxisRaw("P1_Vertical");

            //dash

            //crouch  
        }
    }

    void FixedUpdate()
    {
        if((moveHorizontal > 0.1f && rb2D.velocity.x < maxSpeed)|| (moveHorizontal < 0.1f && rb2D.velocity.x > -maxSpeed))
        {
            rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
        }

        if(moveVertical > 0.1f && !isJumping)
        {
            rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        }
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
