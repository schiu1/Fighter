using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MayControls : PlayerControls
{
    MayCombat combat;
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
            foreach(GameObject o in GameObject.FindGameObjectsWithTag("Player2"))
            {
                if(GameManager.gameManager.p2Name == o.name)
                {
                    enemyPlayer = o;
                    break;
                }
            }
        }
        else
        {
            Player1 = false;
            facingRight = false;
            foreach(GameObject o in GameObject.FindGameObjectsWithTag("Player1"))
            {
                if(GameManager.gameManager.p1Name == o.name)
                {
                    enemyPlayer = o;
                    break;
                }
            }
        }

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        capCollider = gameObject.GetComponent<CapsuleCollider2D>();
        combat = gameObject.GetComponent<MayCombat>();
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
        inCrouchAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("CrouchAttack") && !inCrouchAttack)
        {
            inCrouchAttack = !inCrouchAttack;
            Debug.Log("inCrouchAttack: " + inCrouchAttack);
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("CrouchAttack") && inCrouchAttack)
        {
            inCrouchAttack = !inCrouchAttack;
            Debug.Log("inCrouchAttack: " + inCrouchAttack);
        }

        if (!GameManager.gameManager.isPaused)
        {

            if (isCrouching == true)
            {
                canMove = false;
            }

            if (GameManager.gameManager.timedOut == true 
                || (GameManager.gameManager._p2Health.Health == 0 && Player1)
                || (GameManager.gameManager._p1Health.Health == 0 && !Player1))
            {
                //prevent walking/jumping/crouching from activating
                canMove = false;
                canCrouch = false;
            
                //make character stand still
                moveHorizontal = 0f;
                moveVertical = false;
                animator.SetFloat("Speed", 0);

                //if they are crouching when times out, similar to uncrouch()
                if (isCrouching == true)
                {
                    isCrouching = false;
                    animator.SetBool("IsCrouching", false);
                    capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.54259f);
                    capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.27437781f);
                }

                //do win or lose anim
                if((Player1 && (GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health) && !winAnim) 
                    ||(!Player1 && (GameManager.gameManager._p2Health.Health > GameManager.gameManager._p1Health.Health) && !winAnim))
                {
                    StartCoroutine(WinAnimation());
                    winAnim = true;
                }
                else if ((Player1 && (GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health))
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
                    if (Input.GetButtonDown("P1_Jump") && !isJumping)
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
                if (Player1)
                {
                    if (firstPress == 0f && Input.GetButtonDown("P1_Walk") && airDash == 0)
                    {
                        firstPress = Time.time;
                        direction = moveHorizontal;
                    }
                    else if (Time.time < firstPress + 0.5f && firstPress != 0f && direction == moveHorizontal && Input.GetButtonDown("P1_Walk"))
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
            {
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
            else if (canCrouch && !Player1)
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

    private IEnumerator WinAnimation() //used in Update
    {
        yield return new WaitForSeconds(1.2f);

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        animator.SetBool("Win", true);

        Instantiate(pirate, new Vector2(x + 4, y), Quaternion.identity);
    }

    void WinAnimFaceRight()
    {
        if (facingRight)
        {
            Flip();
        }
    }

    public void WinAnimSpin()
    {
        Debug.Log("winspin");
        animator.SetTrigger("WinSpin");
    }

    public override void Pushback(string pushType)
    {
        if(pushType == "flinch")
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
            Debug.Log(gameObject.transform.position.x+"-"+enemyPlayer.transform.position.x);
            canMove = false;
            canCrouch = false;
            moveHorizontal = 0;
            combat.canAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("Push");
            if(gameObject.transform.position.x - enemyPlayer.transform.position.x > 0)
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
            if(gameObject.transform.position.x - enemyPlayer.transform.position.x > 0)
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

    void PushEnd()
    {
        canMove = true;
        canCrouch = true;
        rb2D.isKinematic = false; // for if player is hit between stopmovement and startmovement like during attack 
        combat.canAttack = true;
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
        /*
         * might not need this anymore as i don't remember its purpose
         * i thought it was for preventing movement while in block anim
         * but i fixed that in another way
        if (!animator.GetBool("IsCrouching"))
        {
            canMove = true;
        }
        */
        canMove = true;
        rb2D.isKinematic = false;
    }

    void Crouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y - 0.54259f); 
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y - 0.27437781f);
        canMove = false;
    }

    void Uncrouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.54259f); 
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.27437781f);
        canMove = true;
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
