using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * make a bool Player1 that is set to either true or false at beginning of Start()
 * remove the P1 in all three of the character scripts (will need to check and fix errors after
 * scan BridgetControls for anything p1 or p2 specific and split them with if else
 * 
 * set default of facingRight to true or false depending on p1 or p2 with if else
 * might not need to change facingRight except for default?
 * moveHorizontal will need if else for p1 and p2
 * change to "canCrouch && Player1" for p1 and make elseif with "canCrouch && !Player1" for p2
 */

public class BridgetControls : PlayerControls
{
    BridgetCombat combat;
    GameObject enemyPlayer;

    bool facingRight;
    bool Player1;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.transform.parent.name == "Player1")
        {
            Player1 = true;
            facingRight = true;
        }
        else
        {
            Player1 = false;
            facingRight = false;
        }

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        capCollider = gameObject.GetComponent<CapsuleCollider2D>();
        combat = gameObject.GetComponent<BridgetCombat>();
        /*
        if (Player1)
        {
            enemyPlayer = GameObject.Find(GameManager.gameManager.p2Name);
            Debug.Log("p2: "+enemyPlayer.name);
        }
        else if (!Player1)
        {
            enemyPlayer = GameObject.Find(GameManager.gameManager.p1Name);
            Debug.Log("p1: "+enemyPlayer.name);
        }
        */
        if (Player1)
        {
            foreach(GameObject o in GameObject.FindGameObjectsWithTag("Player2"))
            {
                if(GameManager.gameManager.p2Name == o.name)
                {
                    enemyPlayer = o;
                    break;
                }
            }
        }
        else if (!Player1)
        {
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Player1"))
            {
                if (GameManager.gameManager.p1Name == o.name)
                {
                    enemyPlayer = o;
                    break;
                }
            }
        }
        speed = 3f;
        maxSpeed = 4f;
        jumpForce = 20f;

        fallMultiplier = 7f;
        isJumping = false;

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

            //MAKE P2 VERSION
            //when times out or gets kill
            if(GameManager.gameManager.timedOut == true 
                || (GameManager.gameManager._p2Health.Health == 0 && Player1) 
                || (GameManager.gameManager._p1Health.Health == 0 && !Player1))
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
                if((Player1 && (GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health) 
                    || (!Player1 && (GameManager.gameManager._p2Health.Health > GameManager.gameManager._p1Health.Health))))
                {
                    //bridget specific win anim
                    StartCoroutine(WinAnimation());
                }
                else if (Player1 && (GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health)
                    || (!Player1 && (GameManager.gameManager._p2Health.Health < GameManager.gameManager._p1Health.Health)))
                {
                    animator.SetBool("Lost", true);
                }
            }

            if (canMove)
            {
                if (Player1)
                {
                    moveHorizontal = Input.GetAxisRaw("P1_Walk");
                    if(Input.GetButtonDown("P1_Jump") && !isJumping)
                    {
                        moveVertical = true;
                    }
                }
                else if (!Player1)
                {
                    moveHorizontal = Input.GetAxisRaw("P2_Walk");
                    if (Input.GetButtonDown("P2_Jump") && !isJumping)
                    {
                        moveVertical = true;
                    }
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
                if (Player1)
                {
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
                else if (!Player1)
                {
                    if (firstPress == 0f && Input.GetButtonDown("P2_Walk") && airDash == 0)
                    {
                        firstPress = Time.time;
                        direction = moveHorizontal;
                    }
                    else if (Time.time < firstPress + 0.5f && firstPress != 0f && direction == moveHorizontal && Input.GetButtonDown("P2_Walk"))
                    {
                        dash = true;
                        firstPress = 0f;
                        direction = 0f;
                    }
                }
            }
            if (canCrouch && Player1)
            {                                                                                    //this is why its not in base class
                if (Input.GetButton("P1_Crouch") && isJumping == false && isCrouching == false && combat.attacking == false)
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
            else if(canCrouch && !Player1)
            {
                if (Input.GetButton("P2_Crouch") && isJumping == false && isCrouching == false && combat.attacking == false)
                {
                    animator.SetTrigger("Crouch");
                    animator.SetBool("IsCrouching", true);
                    isCrouching = true;
                    Crouch();
                    StopMovement();
                }
                else if (!Input.GetButton("P2_Crouch") && isCrouching == true)
                {
                    animator.SetBool("IsCrouching", false);
                    StartMovement();
                    Uncrouch();
                    isCrouching = false;
                }
            }
        }
    }

    IEnumerator WinAnimation()
    {
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("Win", true);
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
            canMove = false;
            canCrouch = false;
            moveHorizontal = 0;
            combat.canAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("Flinch");
        }
        else if (pushType == "push")
        {
            canMove = false;
            canCrouch = false;
            moveHorizontal = 0;
            combat.canAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("Push");
            if (gameObject.transform.position.x - enemyPlayer.transform.position.x > 0)
            {
                if (facingRight) { Flip(); }
                pushForceX = 15f;
                Debug.Log("pushing right");
            }
            else if (gameObject.transform.position.x - enemyPlayer.transform.position.x < 0)
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
            combat.canAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("KDAir");
            if (gameObject.transform.position.x - enemyPlayer.transform.position.x > 0)
            {
                if (facingRight) { Flip(); }
                pushForceX = 15f;
                Debug.Log("KD right");
            }
            else if (gameObject.transform.position.x - enemyPlayer.transform.position.x < 0)
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
        combat.canAttack = true;
    }

    public override void BlockAttack()
    {
        canMove = false;
        canCrouch = false;
        combat.canAttack = false;
        rb2D.isKinematic = false;
        animator.SetTrigger("Block");
        if (gameObject.transform.position.x - enemyPlayer.transform.position.x > 0)
        {
            if (facingRight) { Flip(); }
        }
        else if (gameObject.transform.position.x - enemyPlayer.transform.position.x < 0)
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
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        moveHorizontal = 0f;
        animator.SetFloat("Speed", 0);
    }

    void StartMovement()
    {
        canMove = true;
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
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), enemyPlayer.GetComponent<CapsuleCollider2D>(), false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
            animator.SetBool("InAir", true);
            //might want to change this somehow in the future to ignore collision only on jump start and not entire jump anim
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), enemyPlayer.GetComponent<CapsuleCollider2D>(), true);
        }
    }
}
