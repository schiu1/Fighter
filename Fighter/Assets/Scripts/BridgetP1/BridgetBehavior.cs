using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgetBehavior : PlayerBehavior
{
    BridgetControls controls;
    BridgetCombat combat;
    UnitHealth Health;

    void Awake()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Fight_Scene"))
        {
            this.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.name == "Player1")
        {
            Health = GameManager.gameManager._p1Health;
        }
        else
        {
            Health = GameManager.gameManager._p2Health;
        }
        anim = gameObject.GetComponent<Animator>();
        controls = gameObject.GetComponent<BridgetControls>();
        combat = gameObject.GetComponent<BridgetCombat>();

        startTime = Time.time;
        started = false;

        vcam = GameObject.Find("CM vcam1");
        shake = vcam.GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused)
        {
            if (Time.time - startTime >= 3f && !started)
            {
                started = true;
                controls.canMove = true;
                controls.canCrouch = true;
                combat.canAttack = true;
            }

            //leave out self-heal and self-dmg

            if(Health.Health <= 0)
            {
                Die();
            }
        }
    }

    public override void PlayerDmg(int dmg)
    {
        Health.dmgUnit(dmg);
        _healthbar.SetHealth(Health.Health);
        GameObject b = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(b, .2f); //based on the particle system's duration
        shake.ShakeCamera(.3f, .5f);
    }

    public override void PlayerHeal(int heal)
    {
        Health.healUnit(heal);
        _healthbar.SetHealth(Health.Health);
    }

    protected override void Die()
    {
        anim.SetBool("IsKO", true);
        controls.canMove = false;
        controls.canCrouch = false;
        combat.canAttack = false;
        GameManager.gameManager.endRound("player");
        this.enabled = false;
    }
}
