using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgetCombat : PlayerCombat
{
    BridgetControls controls;
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
        controls = gameObject.GetComponent<BridgetControls>();

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
                            Debug.Log("punch");
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
                            Debug.Log("kick");
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
                            Debug.Log("slash");
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
                            Debug.Log("heavy");
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

    void Punch()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(punchAttackPoint.position, punchAttackRange, 0, enemyLayers);
        
        foreach(Collider2D enemy in enemies)
        {
            if(enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching
                && !enemyControls.inCrouchAttack)
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

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

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

    void Slash()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(slashAttackPoint.position, slashAttackRange, 0, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

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
                AudioManager.audioManager.PlaySound("Punch");

                if ((Player1 && GameManager.gameManager._p2Health.Health > 0)
                    || (!Player1 && GameManager.gameManager._p1Health.Health > 0))
                {
                    Time.timeScale = 0;
                    StartCoroutine(Hitstop(0.15f));
                }
            }

            Vector2 collisionPoint = enemy.ClosestPoint(slashAttackPoint.position);
            GameObject s = Instantiate(punchEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
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

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

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
                AudioManager.audioManager.PlaySound("Punch");

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

    void CPunch()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cPunchAttackPoint.position, cPunchAttackRange, 0, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

            if (enemyControls.isCrouching
                && !enemyControls.inCrouchAttack)
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
                    StartCoroutine(Hitstop(0.2f));
                }
            }

            Vector2 collisionPoint = enemy.ClosestPoint(heavyAttackPoint.position);
            GameObject s = Instantiate(slashEffect, collisionPoint, Quaternion.Euler(new Vector3(0, 0, 90f)));
            Destroy(s, .5f);
        }
    }

    void CKick()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cKickAttackPoint.position, cKickAttackRange, 0, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

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
                AudioManager.audioManager.PlaySound("Punch");

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

    void CSlash()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cSlashAttackPoint.position, cSlashAttackRange, 0, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

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
                AudioManager.audioManager.PlaySound("Punch");

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

    void CHeavySlash()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(cHeavyAttackPoint.position, cHeavyAttackRange, 0, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetType() == typeof(BoxCollider2D))
            {
                continue;
            }

            PlayerControls enemyControls = enemy.GetComponent<PlayerControls>();
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();

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
                    enemyControls.Pushback("flinch");
                }
                AudioManager.audioManager.PlaySound("Punch");

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
