using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Combat : MonoBehaviour
{
    Animator anim;
    P1Controls p1Controls;
    float lastAttack = 0f;
    float attackCD = 0f;

    [SerializeField] Transform punchAttackPoint = null;
    [SerializeField] Vector2 punchAttackRange = Vector2.zero; //0.5583461f, 0.6071799f
    [SerializeField] LayerMask enemyLayers = 0;

    [SerializeField] Transform slashAttackPoint = null;
    [SerializeField] Vector2 slashAttackRange = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        p1Controls = gameObject.GetComponent<P1Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks CD of attacking
        if(Time.time > lastAttack + attackCD)
        {
            lastAttack = 0f;
            attackCD = 0f;
        }

        if (Input.GetButtonDown("P1_Punch") && (p1Controls.isJumping == false) && (attackCD == 0))
        {
            anim.SetTrigger("Punch");
            lastAttack = Time.time;
            attackCD = 0.5f;
        }
        if (Input.GetButtonDown("P1_Kick") && p1Controls.isJumping == false)
        {
            //anim calls kick animation
        }
        if (Input.GetButtonDown("P1_Slash") && p1Controls.isJumping == false && (attackCD == 0))
        {
            anim.SetTrigger("Slash");
            lastAttack = Time.time;
            attackCD = 0.9f;
        }
        if (Input.GetButtonDown("P1_HeavySlash") && p1Controls.isJumping == false)
        {
            //anim calls HS animation
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
            enemy.GetComponent<P2Behavior>().Player2Dmg(10);
        }
    }

    void kick()
    {

    }

    void slash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(slashAttackPoint.position, slashAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("player1 hit: " + enemy.name);
            enemy.GetComponent<P2Behavior>().Player2Dmg(20);
        }
    }

    void heavySlash()
    {

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(punchAttackPoint.position, punchAttackRange);
        Gizmos.DrawWireCube(slashAttackPoint.position, slashAttackRange);
    }
}
