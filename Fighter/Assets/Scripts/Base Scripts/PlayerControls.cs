using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    //need to check which fields need to be labeled protected and which can be left as private
    protected Rigidbody2D rb2D;
    protected Animator animator;
    protected CapsuleCollider2D capCollider;
    //initialize combat script variable here
    //initialize enemy GameObject variable here

    protected float speed;
    protected float maxSpeed;
    protected float moveHorizontal;
    protected bool moveVertical;

    protected float jumpForce;
    protected float fallMultiplier;
    [HideInInspector]
    public bool isJumping;

    protected float firstPress;
    protected bool dash;
    protected float dashForce;
    protected float direction;
    protected int airDash = 0;

    //initialize facing direction bool here
    //initialize public canMove bool here with [HideInInspector]
    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public bool canCrouch;
    [HideInInspector]
    public bool isCrouching;

    protected bool pushback;
    protected float pushForceX;
    protected float pushForceY;
    protected bool KDGround;

    [SerializeField]
    protected GameObject pirate = null;
    protected bool winAnim;

    //reason why this is here is so that child classes don't need to keep defining this
    protected void Awake() 
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Fight_Scene"))
        {
            this.enabled = false;
        }
    }

    // define Start in child classes bc some variables depend if the player is p1 or p2

    // define Update in child classes bc same as Start

    protected void FixedUpdate() //this might be fine to put here?
    {
        if (isJumping == false && KDGround == true)
        {
            animator.SetTrigger("KDGround");
            KDGround = false;
        }
        if (pushback)
        {
            Debug.Log("in pushback");
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(new Vector2(pushForceX, pushForceY), ForceMode2D.Impulse);
            pushForceX = 0;
            pushForceY = 0;
            pushback = false;
        }

        if ((moveHorizontal > 0.1f && rb2D.velocity.x < maxSpeed) || (moveHorizontal < -0.1f && rb2D.velocity.x > -maxSpeed))
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

    protected IEnumerator WinAnimation() //used in Update
    {
        yield return new WaitForSeconds(1.2f);

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        animator.SetBool("Win", true);

        Instantiate(pirate, new Vector2(x + 4, y), Quaternion.identity);
    }

    // WinAnimFaceRight in child classes bc uses facingRight or facingLeft

    public void WinAnimSpin()
    {
        animator.SetTrigger("WinSpin");
    }

    public virtual void Pushback(string pushType)
    {

    }

    public virtual void BlockAttack()
    {

    }

    // Flip in child classes - - - - -

    // PushEnd in child classes bc uses p1 or p2 specific fields

    // stopMovement and startMovement in child classes - - - - - - -

    // crouch and unCrouch uses p1/p2 and the size of the capsule collider differs between fighters

    // OnTriggerEnter2D and OnTriggerExit2D uses p1/p2
}
