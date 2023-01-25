using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    //need to check which fields need to be labeled protected and which can be left as private
    Rigidbody2D rb2D;
    Animator animator;
    CapsuleCollider2D capCollider;
    //initialize combat script variable here
    //initialize enemy GameObject variable here

    float speed;
    float maxSpeed;
    float moveHorizontal;
    bool moveVertical;

    float jumpForce;
    float fallMultiplier;
    [HideInInspector]
    public bool isJumping;

    float firstPress;
    bool dash;
    float dashForce;
    float direction;
    int airDash = 0;

    //initialize facing direction bool here
    //initialize public canMove bool here with [HideInInspector]
    [HideInInspector]
    public bool canCrouch;
    [HideInInspector]
    public bool isCrouching;

    bool pushback;
    float pushForceX;
    float pushForceY;
    bool KDGround;

    [SerializeField]
    GameObject pirate = null;
    bool winAnim;

    protected void Awake()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Fight_Scene"))
        {
            this.enabled = false;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
