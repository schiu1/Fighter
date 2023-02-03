using System.Collections;
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


        }
    }

    void WinAnimFaceRight()
    {

    }

    public void Pushback(string pushType)
    {

    }

    void PushEnd()
    {

    }

    public void BlockAttack()
    {

    }

    void Flip()
    {

    }

    void StopMovement()
    {

    }

    void StartMovement()
    {

    }

    void Crouch()
    {

    }

    void Uncrouch()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

    }

    void OnTriggerExit2D(Collider2D collision)
    {

    }
}
