using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Controls : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator animator;
    CapsuleCollider2D capCollider;
    P2Combat p2combat;
    GameObject p1;

    float speed;
    float maxSpeed;
    float moveHorizontal;
    bool moveVertical;

    float jumpForce;
    float fallMultiplier;
    public bool isJumping;

    float firstPress;
    bool dash;
    float dashForce;
    float direction;
    int airDash = 0;

    public bool facingLeft;
    [HideInInspector]
    public bool p2CanMove; // temp set to true until i implement GameManager
    public bool canCrouch;
    public bool isCrouching;

    bool pushback;
    float pushForceX;
    float pushForceY;
    bool KDGround;

    [SerializeField]
    GameObject pirate = null;
    bool winAnim;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        capCollider = gameObject.GetComponent<CapsuleCollider2D>();
        p2combat = gameObject.GetComponent<P2Combat>();
        p1 = GameObject.Find("Player1");
        speed = 3f;
        maxSpeed = 4f;
        jumpForce = 20f;

        fallMultiplier = 7f;
        isJumping = false;

        facingLeft = true;

        firstPress = 0f;
        dash = false;
        dashForce = 30f;
        direction = 0f;

        p2CanMove = false;
        canCrouch = false;
        isCrouching = false;

        pushback = false;
        pushForceX = 0f;
        pushForceY = 0f;
        KDGround = false;

        winAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrouching == true)
        {
            p2CanMove = false;
        }

        if (GameManager.gameManager.timedOut == true || GameManager.gameManager._p1Health.Health == 0)
        {
            //prevent walking/jumping/crouching from activating
            p2CanMove = false;
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
            if((GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health) && !winAnim)
            {
                //the Win bool as true will make the player go to look at friend on the right anim
                StartCoroutine(WinAnimation());
                winAnim = true;
            }
            else if (GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health)
            {
                animator.SetBool("Lost", true);
            }
        }

        if (p2CanMove)
        {
            //wrap these two around if notjumping
            moveHorizontal = Input.GetAxisRaw("P2_Walk");
            if(Input.GetButtonDown("P2_Jump") && !isJumping)
            {
                moveVertical = true;
            }

            animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

            if (moveHorizontal > 0 && facingLeft)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && !facingLeft)
            {
                Flip();
            }

            //dash
            if (Time.time > firstPress + 0.5f || direction == -moveHorizontal)
            {
                firstPress = 0f;
                direction = 0f;
            }
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

        if (canCrouch)
        {
            if (Input.GetButton("P2_Crouch") && isJumping == false && isCrouching == false && p2combat.p2Attacking == false)
            {
                animator.SetTrigger("Crouch");
                animator.SetBool("IsCrouching", true);
                isCrouching = true;
                crouch();
                stopMovement();
            }
            else if (Input.GetButtonUp("P2_Crouch") && isCrouching == true)
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
        if(isJumping == false && KDGround == true)
        {
            animator.SetTrigger("KDGround");
            KDGround = false;
        }
        if (pushback)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(new Vector2(pushForceX, pushForceY), ForceMode2D.Impulse);
            pushForceX = 0;
            pushForceY = 0;
            pushback = false;
        }

        if (((moveHorizontal > 0.1f && rb2D.velocity.x < maxSpeed) || (moveHorizontal < -0.1f && rb2D.velocity.x > -maxSpeed)) && !pushback)
        {
            rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
        }

        if (dash == true)
        {
            dash = false;
            animator.SetTrigger("Dash");
            rb2D.AddForce(new Vector2(moveHorizontal * dashForce, 0f), ForceMode2D.Impulse);
            if (isJumping == true)
            {
                airDash += 1;
            }
        }

        if (moveVertical == true && !isJumping)
        {
            animator.SetTrigger("Jump");
            rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            moveVertical = false;
        }
        //falling helper
        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }


    }

    IEnumerator WinAnimation()
    {
        //spawn an object to the right of the player with the sprite
        //make new object move left, towards the player
        //when they collide, new object dissapear and set bool Friend to true
        //this will make the player looking at friend to spin with friend
        yield return new  WaitForSeconds(1.2f);

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        animator.SetBool("Win", true);
        GameObject pirateObject = Instantiate(pirate, new Vector2(x + 4, y), Quaternion.identity);
    }

    void WinAnimFaceRight()
    {
        if (!facingLeft)
        {
            Flip();
        }
    }

    public void WinAnimSpin()
    {
        animator.SetTrigger("WinSpin");
    }

    public void Pushback(string pushType)
     {
        if (pushType == "flinch")
        {
            animator.SetTrigger("Flinch");
        }
        else if (pushType == "push")
        {
            p2CanMove = false;
            moveHorizontal = 0;
            p2combat.p2CanAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("Push");
            if (gameObject.transform.position.x - p1.transform.position.x > 0)
            {
                if (!facingLeft) { Flip(); }
                pushForceX = 15f;
                Debug.Log("pushing right");
            }
            else if (gameObject.transform.position.x - p1.transform.position.x < 0)
            {
                if (facingLeft) { Flip(); }
                pushForceX = -15f;
                Debug.Log("pushing left");
            }
            pushback = true;
        }
        else if (pushType == "knockdown") //very similar to push code, might change this later
        {
            p2CanMove = false;
            moveHorizontal = 0;
            p2combat.p2CanAttack = false;
            rb2D.isKinematic = false;
            animator.SetTrigger("KDAir");
            if (gameObject.transform.position.x - p1.transform.position.x > 0)
            {
                if (!facingLeft) { Flip(); }
                pushForceX = 15f;
                Debug.Log("KD right");
            }
            else if (gameObject.transform.position.x - p1.transform.position.x < 0)
            {
                if (facingLeft) { Flip(); }
                pushForceX = -15f;
                Debug.Log("KD left");
            }
            pushForceY = 5f;
            pushback = true;
            KDGround = true;
        }
     }

    public void Flip()
    {
        facingLeft = !facingLeft;
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }

    void PushEnd()
    {
        p2CanMove = true;
        rb2D.isKinematic = false; // for if player is hit between stopmovement and startmovement like during attack 
        p2combat.p2CanAttack = true;
    }

    void stopMovement()
    {
        p2CanMove = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        moveHorizontal = 0f;
        animator.SetFloat("Speed", 0);
    }

    void startMovement()
    {
        p2CanMove = true;
        rb2D.isKinematic = false; //for allowing player to move once again in general cases
    }

    void crouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y - 0.54259f);
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y - 0.27437781f);
        p2CanMove = false;
    }

    void unCrouch()
    {
        capCollider.size = new Vector2(capCollider.size.x, capCollider.size.y + 0.54259f);
        capCollider.offset = new Vector2(capCollider.offset.x, capCollider.offset.y + 0.27437781f);
        p2CanMove = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("InAir", false);
            if (airDash != 0)
            {
                airDash = 0;
            }
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), p1.GetComponent<CapsuleCollider2D>(), false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
            animator.SetBool("InAir", true);
            //might want to change this somehow in the future to ignore collision only on jump start and not entire jump anim
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), p1.GetComponent<CapsuleCollider2D>(), true);
        }
    }
}
