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
        attackList.Add("cPunch", new Attack()
        {
            AttackPoint = cPunchAttackPoint.position,
            AttackRange = cPunchAttackRange,
            AttackDamage = 5,
            PushType = "flinch",
            SoundType = "Punch"
        });
        attackList.Add("cKick", new Attack()
        {
            AttackPoint = cKickAttackPoint.position,
            AttackRange = cKickAttackRange,
            AttackDamage = 10,
            PushType = "flinch",
            SoundType = "Kick"
        });
        attackList.Add("cSlash", new Attack()
        {
            AttackPoint = cSlashAttackPoint.position,
            AttackRange = cSlashAttackRange,
            AttackDamage = 15,
            PushType = "flinch",
            SoundType = "Slash"
        });
        attackList.Add("cHeavy", new Attack()
        {
            AttackPoint = cHeavyAttackPoint.position,
            AttackRange = cHeavyAttackRange,
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
        attackList["punch"].AttackPoint = punchAttackPoint.position;
        attackList["kick"].AttackPoint = kickAttackPoint.position;
        attackList["slash"].AttackPoint = slashAttackPoint.position;
        attackList["heavy"].AttackPoint = heavyAttackPoint.position;
        attackList["cPunch"].AttackPoint = cPunchAttackPoint.position;
        attackList["cKick"].AttackPoint = cKickAttackPoint.position;
        attackList["cSlash"].AttackPoint = cSlashAttackPoint.position;
        attackList["cHeavy"].AttackPoint = cHeavyAttackPoint.position;

        if (!GameManager.gameManager.isPaused)
        {
            //checks CD of p1Attacking
            if (Time.time > lastAttack + attackCD)
            {
                lastAttack = 0f;
                attackCD = 0f;
                attacking = false;
            }

            if (GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0 || GameManager.gameManager._p1Health.Health == 0)
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

        // v change this v
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attPoint, attRange, 0, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            Debug.Log(enemy.name);
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
}