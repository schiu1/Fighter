using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Combat : MonoBehaviour
{
    Animator anim;
    P1Controls p1Controls;
    float lastAttack = 0f;
    float attackCD = 0f;
    public bool p1Attacking;
    public bool p1CanAttack;
    [SerializeField] LayerMask enemyLayers = 0;

    [SerializeField] Transform punchAttackPoint = null;
    [SerializeField] Vector2 punchAttackRange = Vector2.zero; //0.5583461f, 0.6071799f

    [SerializeField] Transform slashAttackPoint = null;
    [SerializeField] Vector2 slashAttackRange = Vector2.zero;

    [SerializeField] Transform heavyAttackPoint = null;
    [SerializeField] Vector2 heavyAttackRange = Vector2.zero;

    [SerializeField] Transform kickAttackPoint = null;
    [SerializeField] Vector2 kickAttackRange = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        p1Controls = gameObject.GetComponent<P1Controls>();

        p1Attacking = false;
        p1CanAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks CD of p1Attacking
        if(Time.time > lastAttack + attackCD)
        {
            lastAttack = 0f;
            attackCD = 0f;
            p1Attacking = false;
        }

        if(GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0)
        {
            p1CanAttack = false;
        }

        if (p1CanAttack)
        {
            if ((p1Controls.isJumping == false) && (attackCD == 0) && (p1Controls.isCrouching == false))
            {
                if (Input.GetButtonDown("P1_Punch"))
                {
                    p1Attacking = true;
                    anim.SetTrigger("Punch");
                    lastAttack = Time.time;
                    attackCD = 0.5f;
                }
                if (Input.GetButtonDown("P1_Kick"))
                {
                    p1Attacking = true;
                    anim.SetTrigger("Kick");
                    lastAttack = Time.time;
                    attackCD = 0.8f;
                }
                if (Input.GetButtonDown("P1_Slash"))
                {
                    p1Attacking = true;
                    anim.SetTrigger("Slash");
                    lastAttack = Time.time;
                    attackCD = 0.8f;
                }
                if (Input.GetButtonDown("P1_HeavySlash"))
                {
                    p1Attacking = true;
                    anim.SetTrigger("Heavy");
                    lastAttack = Time.time;
                    attackCD = 0.75f;
                }
            }
        }

    }

    //put these methods in attack anim as events
    void punch()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(punchAttackPoint.position, punchAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach(Collider2D enemy in enemies)
        {
            Debug.Log("player1 hit: " + enemy.name);
            enemy.GetComponent<P2Behavior>().Player2Dmg(5, "flinch");
        }
    }

    void kick()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(kickAttackPoint.position, kickAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player1 hit: " + enemy.name);
            enemy.GetComponent<P2Behavior>().Player2Dmg(10, "pushback");
        }
    }

    void slash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(slashAttackPoint.position, slashAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player1 hit: " + enemy.name);
            enemy.GetComponent<P2Behavior>().Player2Dmg(15, "flinch");
        }
    }

    void heavySlash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(heavyAttackPoint.position, heavyAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player1 hit: " + enemy.name);
            enemy.GetComponent<P2Behavior>().Player2Dmg(20, "pushback");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(punchAttackPoint.position, punchAttackRange);
        Gizmos.DrawWireCube(slashAttackPoint.position, slashAttackRange);
        Gizmos.DrawWireCube(heavyAttackPoint.position, heavyAttackRange);
        Gizmos.DrawWireCube(kickAttackPoint.position, kickAttackRange);
    }
}
