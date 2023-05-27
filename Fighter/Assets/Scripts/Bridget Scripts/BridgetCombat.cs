using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgetCombat : PlayerCombat
{
    BridgetControls controls;
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
        controls = gameObject.GetComponent<BridgetControls>();

        attacking = false;
        canAttack = false;

        attackList.Add("punch", new Attack()
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
            //check CD of attacking
            if(Time.time > lastAttack + attackCD)
            {
                lastAttack = 0f;
                attackCD = 0f;
                attacking = false;
            }

            //prevent attacking when timed out or won fight
            if (GameManager.gameManager.timedOut == true || GameManager.gameManager._p2Health.Health == 0 || GameManager.gameManager._p1Health.Health == 0)
            {
                canAttack = false;
            }

            if (canAttack)
            {
                if((controls.isJumping == false) && (attackCD == 0))
                {
                    if (Player1)
                    {
                        if (Input.GetButtonDown("P1_Punch"))
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
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 0.5f; //will be different
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
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 0.8f; //will be different
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
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 0.8f; //will be different
                            }
                        }
                        if (Input.GetButtonDown("P1_HeavySlash"))
                        {
                            attacking = true;
                            anim.SetTrigger("Heavy");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching"))
                            {
                                attackCD = 1.2f;
                            }
                            else
                            {
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 1.2f; //will be different
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
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 0.5f; //will be different
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
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 0.8f; //will be different
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
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 0.8f; //will be different
                            }
                        }
                        if (Input.GetButtonDown("P2_HeavySlash"))
                        {
                            attacking = true;
                            anim.SetTrigger("Heavy");
                            lastAttack = Time.time;
                            if (!anim.GetBool("IsCrouching"))
                            {
                                attackCD = 1.2f;
                            }
                            else
                            {
                                controls.canMove = false;
                                controls.canCrouch = false;
                                attackCD = 1.2f; //will be different
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
