using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MayCombat : PlayerCombat
{
    MayControls p1Controls;
    bool Player1;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.name == "Player1")
        {
            Player1 = true;
        }
        else
        {
            Player1 = false;
        }
        anim = gameObject.GetComponent<Animator>();
        p1Controls = gameObject.GetComponent<MayControls>();

        attacking = false;
        canAttack = false;
    }

    /*
     * connect animator component 
     * use GetBool from animator component to see if crouching while pressing an attack button
     * if (attackbutton && !isCrouching) -> do standing attack
     * else if (attackbutton && isCrouching) -> do crouching attack
     * connect crouching attack anim to after crouch anim only and returns to crouching_idle when done
     * probably similar logic for air attack too
     * make sure to edit StartMovement to check if fighter is crouching after the attack
     */

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
                attacking = false;
            }

            if(GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0 || GameManager.gameManager._p1Health.Health == 0)
            {
                canAttack = false;
            }

            if (canAttack)
            {
                if ((p1Controls.isJumping == false) && (attackCD == 0))
                {
                    if (Player1)
                    {
                        if (Input.GetButtonDown("P1_Punch") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Punch");
                            lastAttack = Time.time;
                            attackCD = 0.5f;
                        }
                        else if (Input.GetButtonDown("P1_Punch") && anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Punch");
                            lastAttack = Time.time;
                            attackCD = .5f;
                        }

                        if (Input.GetButtonDown("P1_Kick") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Kick");
                            lastAttack = Time.time;
                            attackCD = 0.8f;
                        }
                        else if (Input.GetButtonDown("P1_Kick") && anim.GetBool("IsCrouching"))
                        {

                        }

                        if (Input.GetButtonDown("P1_Slash") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Slash");
                            lastAttack = Time.time;
                            attackCD = 0.8f;
                        }
                        if (Input.GetButtonDown("P1_HeavySlash") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Heavy");
                            lastAttack = Time.time;
                            attackCD = 0.75f;
                        }
                    }
                    else if (!Player1)
                    {
                        if (Input.GetButtonDown("P2_Punch") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Punch");
                            lastAttack = Time.time;
                            attackCD = 0.5f;
                        }
                        else if (Input.GetButtonDown("P2_Punch") && anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Punch");
                            lastAttack = Time.time;
                            attackCD = .5f;
                        }

                        if (Input.GetButtonDown("P2_Kick") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Kick");
                            lastAttack = Time.time;
                            attackCD = 0.8f;
                        }
                        else if (Input.GetButtonDown("P1_Kick") && anim.GetBool("IsCrouching"))
                        {

                        }

                        if (Input.GetButtonDown("P2_Slash") && !anim.GetBool("IsCrouching"))
                        {
                            attacking = true;
                            anim.SetTrigger("Slash");
                            lastAttack = Time.time;
                            attackCD = 0.8f;
                        }
                        if (Input.GetButtonDown("P2_HeavySlash") && !anim.GetBool("IsCrouching"))
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

            if (enemy.GetComponent<PlayerControls>().isCrouching 
                && !enemy.GetComponent<PlayerControls>().inCrouchAttack)
            {
                enemy.GetComponent<PlayerControls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
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

                if((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
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

    void CPunch()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cPunchAttackPoint.position, cPunchAttackRange, 0, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            if (enemy.GetComponent<PlayerControls>().isCrouching 
                && !enemy.GetComponent<PlayerControls>().inCrouchAttack)
            {
                enemy.GetComponent<PlayerControls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }

            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
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

                if ((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
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

            if (enemy.GetComponent<PlayerControls>().isCrouching
                && !enemy.GetComponent<PlayerControls>().inCrouchAttack)
            {
                enemy.GetComponent<PlayerControls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemy.GetComponent<PlayerBehavior>().PlayerDmg(10);
                if (enemy.GetComponent<PlayerControls>().isJumping)
                {
                    enemy.GetComponent<PlayerControls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<PlayerControls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Kick");

                if ((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
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

            if (enemy.GetComponent<PlayerControls>().isCrouching
                && !enemy.GetComponent<PlayerControls>().inCrouchAttack)
            {
                enemy.GetComponent<PlayerControls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemy.GetComponent<PlayerBehavior>().PlayerDmg(15);
                if (enemy.GetComponent<PlayerControls>().isJumping)
                {
                    enemy.GetComponent<PlayerControls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<PlayerControls>().Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Slash");

                if ((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
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

            if (enemy.GetComponent<PlayerControls>().isCrouching
                && !enemy.GetComponent<PlayerControls>().inCrouchAttack)
            {
                enemy.GetComponent<PlayerControls>().BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemy.GetComponent<PlayerBehavior>().PlayerDmg(20);
                if (enemy.GetComponent<PlayerControls>().isJumping)
                {
                    enemy.GetComponent<PlayerControls>().Pushback("knockdown");
                }
                else
                {
                    enemy.GetComponent<PlayerControls>().Pushback("push");
                }
                AudioManager.audioManager.PlaySound("HeavySlash");

                if ((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
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
