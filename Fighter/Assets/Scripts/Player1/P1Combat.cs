using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P1Combat : PlayerCombat
{
    P1Controls p1Controls;

    [HideInInspector]
    public bool p1Attacking;
    [HideInInspector]
    public bool p1CanAttack;
    
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
        if (!GameManager.gameManager.isPaused)
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

    }

    //put these methods in attack anim as events
    void punch()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(punchAttackPoint.position, punchAttackRange, 0, enemyLayers);
        
        //apply damage to enemy
        foreach(Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            if (enemy.GetComponent<P2Controls>().isCrouching)
            {
                enemy.GetComponent<P2Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player1 hit: " + enemy.name);
                enemy.GetComponent<P2Behavior>().PlayerDmg(5);
                if (enemy.GetComponent<P2Controls>().isJumping)
                {
                    enemy.GetComponent<P2Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P2Controls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Punch");

                if(GameManager.gameManager._p2Health.Health > 0)
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.1f));
                }
            }

            Vector2 collisionPoint = enemy.ClosestPoint(punchAttackPoint.position);
            GameObject s = Instantiate(punchEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
            Destroy(s, .5f);
        }
    }

    void kick()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(kickAttackPoint.position, kickAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            if (enemy.GetComponent<P2Controls>().isCrouching)
            {
                enemy.GetComponent<P2Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player1 hit: " + enemy.name);
                enemy.GetComponent<P2Behavior>().PlayerDmg(10);
                if (enemy.GetComponent<P2Controls>().isJumping)
                {
                    enemy.GetComponent<P2Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P2Controls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Kick");

                if (GameManager.gameManager._p2Health.Health > 0)
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.1f));
                }
            }

            Vector2 collisionPoint = enemy.ClosestPoint(kickAttackPoint.position);
            GameObject s = Instantiate(punchEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
            Destroy(s, .5f);
        }
    }

    void slash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(slashAttackPoint.position, slashAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            if (enemy.GetComponent<P2Controls>().isCrouching)
            {
                enemy.GetComponent<P2Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player1 hit: " + enemy.name);
                enemy.GetComponent<P2Behavior>().PlayerDmg(15);
                if (enemy.GetComponent<P2Controls>().isJumping)
                {
                    enemy.GetComponent<P2Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P2Controls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Slash");

                if (GameManager.gameManager._p2Health.Health > 0)
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.15f));
                }
            }

            Vector2 collisionPoint = enemy.ClosestPoint(slashAttackPoint.position);
            GameObject s = Instantiate(slashEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
            Destroy(s, .5f);
        }
    }

    void heavySlash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(heavyAttackPoint.position, heavyAttackRange, 0, enemyLayers);
        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            if(enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            if (enemy.GetComponent<P2Controls>().isCrouching)
            {
                enemy.GetComponent<P2Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player1 hit: " + enemy.name);
                enemy.GetComponent<P2Behavior>().PlayerDmg(20);
                if (enemy.GetComponent<P2Controls>().isJumping)
                {
                    enemy.GetComponent<P2Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P2Controls>().Pushback("push");
                }
                AudioManager.audioManager.PlaySound("HeavySlash");

                if (GameManager.gameManager._p2Health.Health > 0)
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.2f));
                }
            }

            Vector2 collisionPoint = enemy.ClosestPoint(heavyAttackPoint.position);
            GameObject s = Instantiate(slashEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 90f)));
            Destroy(s, .5f);
        }
    }
}
