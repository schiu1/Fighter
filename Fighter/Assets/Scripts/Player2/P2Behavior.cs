using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P2Behavior : PlayerBehavior
{
    P2Controls p2controls;
    P2Combat p2combat;
    UnitHealth p2Health;

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
        p2Health = GameManager.gameManager._p2Health;
        anim = gameObject.GetComponent<Animator>();
        p2controls = gameObject.GetComponent<P2Controls>();
        p2combat = gameObject.GetComponent<P2Combat>();

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
                p2controls.canMove = true;
                p2controls.canCrouch = true;
                p2combat.canAttack = true;
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                PlayerDmg(10);
                Debug.Log("player2: " + GameManager.gameManager._p2Health.Health);
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                PlayerHeal(10);
                Debug.Log("player2: " + GameManager.gameManager._p2Health.Health);
            }

            //if player reaches 0 health, call Die()
            if(p2Health.Health <= 0)
            {
                Die();
            }
        }
    }

    /*
    for pushing back player when hit,
    need to first determine where the enemy is by using transform.playerposition - transform.enemyposition
    if negative, that means enemy is on the right
    if positive, that means enemy is on the left
    so push direction will be opposite of the direction of the enemy
    now get some sort of pushForce value and use it in AddForce(vector2, Impulse) on the player
    while AddForce is called, trigger pushback animation and prevent movement and attack from player
    */

    public override void PlayerDmg(int dmg)
    {
        p2Health.dmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._p2Health.Health);
        GameObject b = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(b, .2f); //based on the particle system's duration  
        shake.ShakeCamera(.3f, .5f);
    }

    public override void PlayerHeal(int heal)
    {
        p2Health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p2Health.Health);
    }

    protected override void Die()
    {
        Debug.Log("p2 killed");
        anim.SetBool("IsKO", true);
        p2controls.canMove = false;
        p2controls.canCrouch = false;
        p2combat.canAttack = false;
        GameManager.gameManager.endRound("player");
        this.enabled = false;
    }

}
