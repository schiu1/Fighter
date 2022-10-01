using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Combat : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform attackPoint;
    //[SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("P1_Punch"))
        {
            anim.SetTrigger("Punch");
        }
        if (Input.GetButtonDown("P1_Kick"))
        {
            //anim calls kick animation
            kick();
        }
        if (Input.GetButtonDown("P1_Slash"))
        {
            //anim calls slash animation
            slash();
        }
        if (Input.GetButtonDown("P1_HeavySlash"))
        {
            //anim calls HS animation
            heavySlash();
        }
    }

    //put these methods in attack anim as events
    void punch()
    {

        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(0.5583461f, 0.6071799f), enemyLayers);

        //apply damage to enemy
        foreach(Collider2D enemy in enemies)
        {
            //issue here
            enemy.GetComponent<P2Behavior>().Player2Dmg(10);
        }
    }

    void kick()
    {

    }

    void slash()
    {

    }

    void heavySlash()
    {

    }
}
