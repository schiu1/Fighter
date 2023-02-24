using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgetP1Combat : PlayerCombat
{
    BridgetP1Controls p1Controls;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        p1Controls = gameObject.GetComponent<BridgetP1Controls>();

        attacking = false;
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused)
        {
            //check CD of attacking
            if(Time.time > lastAttack + attackCD)
            {
                lastAttack = 0f;
                attackCD = 0f;
                attacking = false;
            }

            //prevent attacking when timed out or won fight
            if (GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0)
            {
                canAttack = false;
            }

            if (canAttack)
            {
                if((p1Controls.isJumping == false) && (attackCD == 0) && (p1Controls.isCrouching == false))
                {
                    if (Input.GetButtonDown("P1_Punch"))
                    {
                        Debug.Log("punch");
                        attacking = true;
                        anim.SetTrigger("Punch");
                        lastAttack = Time.time;
                        attackCD = 0.5f; //will be different
                    }
                    if (Input.GetButtonDown("P1_Kick"))
                    {
                        Debug.Log("kick");
                        attacking = true;
                        anim.SetTrigger("Kick");
                        lastAttack = Time.time;
                        attackCD = 0.8f; //will be different
                    }
                    if (Input.GetButtonDown("P1_Slash"))
                    {
                        Debug.Log("slash");
                        attacking = true;
                        anim.SetTrigger("Slash");
                        lastAttack = Time.time;
                        attackCD = 0.8f; //will be different
                    }
                    if (Input.GetButtonDown("P1_HeavySlash"))
                    {
                        Debug.Log("heavy");
                        attacking = true;
                        anim.SetTrigger("Heavy");
                        lastAttack = Time.time;
                        attackCD = 0.75f; //will be different
                    }
                }
            }
        }
    }

    void Punch()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(punchAttackPoint.position, punchAttackRange, 0, enemyLayers);
        
        foreach(Collider2D enemy in enemies)
        {
            if(enemy.GetType() == typeof(BoxCollider2D))
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

                if (GameManager.gameManager._p2Health.Health > 0)
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.1f));
                }
            }

            //to spawn particle effect
            Vector2 collisionPoint = enemy.ClosestPoint(punchAttackPoint.position);
            GameObject s = Instantiate(punchEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
            Destroy(s, .5f);
        }
    }

    void Kick()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(kickAttackPoint.position, kickAttackRange, 0, enemyLayers);

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
                Debug.Log(gameObject.name + "hit: " + enemy.name);
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

    void Slash()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(slashAttackPoint.position, slashAttackRange, 0, enemyLayers);

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
                AudioManager.audioManager.PlaySound("Punch");

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

    void HeavySlash()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(heavyAttackPoint.position, heavyAttackRange, 0, enemyLayers);

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
                AudioManager.audioManager.PlaySound("Punch");

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
