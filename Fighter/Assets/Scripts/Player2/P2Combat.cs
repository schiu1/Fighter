using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Combat : MonoBehaviour
{
    Animator anim;
    P2Controls p2Controls;
    float lastAttack = 0f;
    float attackCD = 0f;
    public bool p2Attacking;
    public bool p2CanAttack;
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
        p2Controls = gameObject.GetComponent<P2Controls>();

        p2Attacking = false;
        p2CanAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks CD of p1Attacking
        if (Time.time > lastAttack + attackCD)
        {
            lastAttack = 0f;
            attackCD = 0f;
            p2Attacking = false;
        }

        if (GameManager.gameManager.timedOut == true || GameManager.gameManager._p1Health.Health == 0)
        {
            p2CanAttack = false;
        }

        if (p2CanAttack)
        {
            if ((p2Controls.isJumping == false) && (attackCD == 0) && (p2Controls.isCrouching == false))
            {
                if (Input.GetButtonDown("P2_Punch"))
                {
                    p2Attacking = true;
                    anim.SetTrigger("Punch");
                    lastAttack = Time.time;
                    attackCD = 0.5f;
                }
                if (Input.GetButtonDown("P2_Kick"))
                {
                    p2Attacking = true;
                    anim.SetTrigger("Kick");
                    lastAttack = Time.time;
                    attackCD = 0.8f;
                }
                if (Input.GetButtonDown("P2_Slash"))
                {
                    p2Attacking = true;
                    anim.SetTrigger("Slash");
                    lastAttack = Time.time;
                    attackCD = 0.8f;
                }
                if (Input.GetButtonDown("P2_HeavySlash"))
                {
                    p2Attacking = true;
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
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player2 hit: " + enemy.name);
            enemy.GetComponent<P1Behavior>().Player1Dmg(5);
            if (enemy.GetComponent<P1Controls>().isJumping)
            {
                enemy.GetComponent<P1Controls>().Pushback("knockdown");
            }
            else
            {
                enemy.GetComponent<P1Controls>().Pushback("flinch");
            }
        }
    }

    void kick()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(kickAttackPoint.position, kickAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player2 hit: " + enemy.name);
            enemy.GetComponent<P1Behavior>().Player1Dmg(10);
            if (enemy.GetComponent<P1Controls>().isJumping)
            {
                enemy.GetComponent<P1Controls>().Pushback("knockdown");
            }
            else
            {
                enemy.GetComponent<P1Controls>().Pushback("flinch");
            }
        }
    }

    void slash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(slashAttackPoint.position, slashAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player2 hit: " + enemy.name);
            enemy.GetComponent<P1Behavior>().Player1Dmg(15);
            if (enemy.GetComponent<P1Controls>().isJumping)
            {
                enemy.GetComponent<P1Controls>().Pushback("knockdown");
            }
            else
            {
                enemy.GetComponent<P1Controls>().Pushback("flinch");
            }
        }
    }

    void heavySlash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(heavyAttackPoint.position, heavyAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player2 hit: " + enemy.name);
            enemy.GetComponent<P1Behavior>().Player1Dmg(20);
            if (enemy.GetComponent<P1Controls>().isJumping)
            {
                enemy.GetComponent<P1Controls>().Pushback("knockdown");
            }
            else
            {
                enemy.GetComponent<P1Controls>().Pushback("push");
            }
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
