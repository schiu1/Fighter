using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MayBehavior : PlayerBehavior
{
    MayControls controls;
    MayCombat combat;
    UnitHealth health;

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
        if(gameObject.transform.parent.name == "Player1")
        {
            health = GameManager.gameManager._p1Health;
            _healthbar = GameObject.Find("P1_HB_Fill").GetComponent<Healthbar>();
        }
        else
        {
            health = GameManager.gameManager._p2Health;
            _healthbar = GameObject.Find("P2_HB_Fill").GetComponent<Healthbar>();
        }
        anim = gameObject.GetComponent<Animator>();
        controls = gameObject.GetComponent<MayControls>();
        combat = gameObject.GetComponent<MayCombat>();

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
            if(Time.time - startTime >= 3f && !started)
            {
                started = true;
                controls.canMove = true;
                controls.canCrouch = true;
                combat.canAttack = true;
            }

            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerDmg(10);
            
                Debug.Log("player1: " + GameManager.gameManager._p1Health.Health);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                PlayerHeal(10);
                Debug.Log("player1: " + GameManager.gameManager._p1Health.Health);
            }
            */

            if(health.Health <= 0)
            {
                Die();
            }
        }
    }

    public override void PlayerDmg(int dmg)
    {
        health.dmgUnit(dmg);
        _healthbar.SetHealth(health.Health);
        GameObject b = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(b, .2f); //based on the particle system's duration
        shake.ShakeCamera(.3f, .5f);
    }

    public override void PlayerHeal(int heal)
    {
        health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
    }

    protected override void Die()
    {
        Debug.Log("p1 killed");
        anim.SetBool("IsKO", true);
        controls.canMove = false;
        controls.canCrouch = false;
        combat.canAttack = false;
        GameManager.gameManager.endRound("player");
        this.enabled = false;
    }
}
