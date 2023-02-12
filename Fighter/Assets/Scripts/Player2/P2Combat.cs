using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P2Combat : PlayerCombat
{
    P2Controls p2Controls;

    /*
    [HideInInspector]
    public bool p2Attacking;
    [HideInInspector]
    public bool p2CanAttack;
    */

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        p2Controls = gameObject.GetComponent<P2Controls>();

        attacking = false;
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused)
        {
            //checks CD of p1Attacking
            if (Time.time > lastAttack + attackCD)
            {
                lastAttack = 0f;
                attackCD = 0f;
                attacking = false;
            }

            if (GameManager.gameManager.timedOut == true || GameManager.gameManager._p1Health.Health == 0)
            {
                canAttack = false;
            }

            if (canAttack)
            {
                if ((p2Controls.isJumping == false) && (attackCD == 0) && (p2Controls.isCrouching == false))
                {
                    if (Input.GetButtonDown("P2_Punch"))
                    {
                        attacking = true;
                        anim.SetTrigger("Punch");
                        lastAttack = Time.time;
                        attackCD = 0.5f;
                    }
                    if (Input.GetButtonDown("P2_Kick"))
                    {
                        attacking = true;
                        anim.SetTrigger("Kick");
                        lastAttack = Time.time;
                        attackCD = 0.8f;
                    }
                    if (Input.GetButtonDown("P2_Slash"))
                    {
                        attacking = true;
                        anim.SetTrigger("Slash");
                        lastAttack = Time.time;
                        attackCD = 0.8f;
                    }
                    if (Input.GetButtonDown("P2_HeavySlash"))
                    {
                        attacking = true;
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
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }
            if (enemy.GetComponent<PlayerControls>().isCrouching)
            {
                enemy.GetComponent<PlayerControls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player2 hit: " + enemy.name);
                enemy.GetComponent<PlayerBehavior>().PlayerDmg(5);
                if (enemy.GetComponent<PlayerControls>().isJumping)
                {
                    enemy.GetComponent<PlayerControls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<PlayerControls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Punch");

                if (GameManager.gameManager._p1Health.Health > 0)
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

            if (enemy.GetComponent<P1Controls>().isCrouching)
            {
                enemy.GetComponent<P1Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player2 hit: " + enemy.name);
                enemy.GetComponent<P1Behavior>().PlayerDmg(10);
                if (enemy.GetComponent<P1Controls>().isJumping)
                {
                    enemy.GetComponent<P1Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P1Controls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Kick");

                if (GameManager.gameManager._p1Health.Health > 0)
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

            if (enemy.GetComponent<P1Controls>().isCrouching)
            {
                enemy.GetComponent<P1Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player2 hit: " + enemy.name);
                enemy.GetComponent<P1Behavior>().PlayerDmg(15);
                if (enemy.GetComponent<P1Controls>().isJumping)
                {
                    enemy.GetComponent<P1Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P1Controls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Slash");

                if (GameManager.gameManager._p1Health.Health > 0)
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
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            if (enemy.GetComponent<P1Controls>().isCrouching)
            {
                enemy.GetComponent<P1Controls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log("player2 hit: " + enemy.name);
                enemy.GetComponent<P1Behavior>().PlayerDmg(20);
                if (enemy.GetComponent<P1Controls>().isJumping)
                {
                    enemy.GetComponent<P1Controls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<P1Controls>().Pushback("push");
                }
                AudioManager.audioManager.PlaySound("HeavySlash");

                if (GameManager.gameManager._p1Health.Health > 0)
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
