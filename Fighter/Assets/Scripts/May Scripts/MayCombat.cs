using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MayCombat : PlayerCombat
{
    MayControls p1Controls;
    bool Player1;

    Dictionary<string, Attack> attackList = new Dictionary<string, Attack>();

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

        attackList.Add("punch", new Attack() //make this for every kind of attack
        {
            AttackPoint = punchAttackPoint.position,
            AttackRange = punchAttackRange,
            AttackDamage = 5,
            PushType = "flinch",
            SoundType = "Punch"
        });
        attackList.Add("kick", new Attack()
        {
            AttackPoint = kickAttackPoint.position,
            AttackRange = kickAttackRange,
            AttackDamage = 10,
            PushType = "flinch",
            SoundType = "Kick"
        });
        attackList.Add("slash", new Attack()
        {
            AttackPoint = slashAttackPoint.position,
            AttackRange = slashAttackRange,
            AttackDamage = 15,
            PushType = "flinch",
            SoundType = "Slash"
        });
        attackList.Add("heavy", new Attack()
        {
            AttackPoint = heavyAttackPoint.position,
            AttackRange = heavyAttackRange,
            AttackDamage = 20,
            PushType = "push",
            SoundType = "HeavySlash"
        });
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
        attackList["punch"] = new Attack() //make this for every kind of attack
        {
            AttackPoint = punchAttackPoint.position,
            AttackRange = punchAttackRange,
            AttackDamage = 5,
            PushType = "flinch",
            SoundType = "Punch"
        };
        attackList["kick"] = new Attack()
        {
            AttackPoint = kickAttackPoint.position,
            AttackRange = kickAttackRange,
            AttackDamage = 10,
            PushType = "flinch",
            SoundType = "Kick"
        };
        attackList["slash"] = new Attack()
        {
            AttackPoint = slashAttackPoint.position,
            AttackRange = slashAttackRange,
            AttackDamage = 15,
            PushType = "flinch",
            SoundType = "Slash"
        };
        attackList["heavy"] = new Attack()
        {
            AttackPoint = heavyAttackPoint.position,
            AttackRange = heavyAttackRange,
            AttackDamage = 20,
            PushType = "push",
            SoundType = "HeavySlash"
        };

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
                        if (Input.GetButtonDown("P1_Punch"))
                        {
                            attacking = true;
                            anim.SetTrigger("Punch");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching")) //if standing
                            {
                                //preventing movement is in the animation
                                attackCD = 0.5f;
                            }
                            else //if crouching
                            {
                                //preventing movement has to be done here
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;
                                
                                attackCD = 0.5f;
                            }
                        }

                        if (Input.GetButtonDown("P1_Kick"))
                        {
                            attacking = true;
                            anim.SetTrigger("Kick");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching"))
                            {
                                attackCD = 0.8f;
                            }
                            else
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 0.5f;
                            }
                        }

                        if (Input.GetButtonDown("P1_Slash"))
                        {
                            attacking = true;
                            anim.SetTrigger("Slash");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching"))
                            {
                                attackCD = 0.8f;
                            }
                            else 
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 0.8f;
                            }
                        }

                        if (Input.GetButtonDown("P1_HeavySlash"))
                        {
                            attacking = true;
                            anim.SetTrigger("Heavy");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching"))
                            {
                                attackCD = 0.75f;
                            }
                            else 
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 1f;
                            }
                        }              
                    }
                    else if (!Player1)
                    {
                        if (Input.GetButtonDown("P2_Punch"))
                        {
                            attacking = true;
                            anim.SetTrigger("Punch");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching")) 
                            {
                                attackCD = 0.5f;
                            }
                            else 
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 0.5f;
                            }
                        }

                        if (Input.GetButtonDown("P2_Kick"))
                        {
                            attacking = true;
                            anim.SetTrigger("Kick");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching"))
                            {
                                attackCD = 0.8f;
                            }
                            else 
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 0.5f;
                            }
                        }

                        if (Input.GetButtonDown("P2_Slash"))
                        {
                            attacking = true;
                            anim.SetTrigger("Slash");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching")) 
                            {
                                attackCD = 0.8f;
                            }
                            else 
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 0.8f;
                            }
                        }

                        if (Input.GetButtonDown("P2_HeavySlash"))
                        {
                            attacking = true;
                            anim.SetTrigger("Heavy");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching")) 
                            {
                                attackCD = 0.75f;
                            }
                            else 
                            {
                                p1Controls.canMove = false;
                                p1Controls.canCrouch = false;

                                attackCD = 1f;
                            }
                        }
                    }
                }
            }
        }

    }

    void attack(string type)
    {
        Vector3 attPoint = attackList[type].AttackPoint;
        Vector2 attRange = attackList[type].AttackRange;
        int attDmg = attackList[type].AttackDamage;
        string pushType = attackList[type].PushType;
        string soundType = attackList[type].SoundType;
        
        Debug.Log(attPoint);
        Debug.Log(attRange);
        Debug.Log(attDmg);
        Debug.Log(pushType);
        Debug.Log(soundType);
        
        Debug.Log(enemyLayers.value);
        // v change this v
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attPoint, attRange, 0, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(attDmg); 
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback(pushType); 
                }
                AudioManager.audioManager.PlaySound(soundType); 

                if ((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.1f));
                }
            }
        }
    }
    /*
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

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(5);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("flinch");
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

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(10);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("flinch");
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

            var enemyControls = enemy.GetComponent<PlayerControls>();
            var enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(15);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("flinch");
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

            var enemyControls = enemy.GetComponent<PlayerControls>();
            var enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(20);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("push");
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
    }*/

    void CPunch()
    {
        Debug.Log(enemyLayers.value);
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cPunchAttackPoint.position, cPunchAttackRange, 0, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            var enemyControls = enemy.GetComponent<PlayerControls>();
            var enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(5);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("flinch");
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

    void CKick()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cKickAttackPoint.position, cKickAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            var enemyControls = enemy.GetComponent<PlayerControls>();
            var enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching
                && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(10);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("flinch");
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

    void CSlash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cSlashAttackPoint.position, cSlashAttackRange, 0, enemyLayers);

        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            var enemyControls = enemy.GetComponent<PlayerControls>();
            var enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching
                && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(15);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("flinch");
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

    void CHeavySlash()
    {
        //get enemies in range of attack
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cHeavyAttackPoint.position, cHeavyAttackRange, 0, enemyLayers);
        //apply damage to enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            var enemyControls = enemy.GetComponent<PlayerControls>();
            var enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching
                && !enemyControls.inCrouchAttack)
            {
                enemyControls.BlockAttack();
                AudioManager.audioManager.PlaySound("BlockAttack");
            }
            else
            {
                Debug.Log(gameObject.name + " hit: " + enemy.name);
                enemyBehavior.PlayerDmg(20);
                if (enemyControls.isJumping)
                {
                    enemyControls.Pushback("knockdown");
                }
                else
                {
                    enemyControls.Pushback("push");
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
