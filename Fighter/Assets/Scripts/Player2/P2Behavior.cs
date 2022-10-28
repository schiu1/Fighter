using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Behavior : MonoBehaviour
{
    P2Controls p2controls;
    P2Combat p2combat;
    Animator anim;
    UnitHealth p2Health;
    [SerializeField] Healthbar _healthbar = null;

    float startTime;
    bool started;
    // Start is called before the first frame update
    void Start()
    {
        p2Health = GameManager.gameManager._p2Health;
        anim = gameObject.GetComponent<Animator>();
        p2controls = gameObject.GetComponent<P2Controls>();
        p2combat = gameObject.GetComponent<P2Combat>();

        startTime = Time.time;
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= 3f && !started)
        {
            started = true;
            p2controls.p2CanMove = true;
            p2controls.canCrouch = true;
            p2combat.p2CanAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            Player2Dmg(10);
            Debug.Log("player2: " + GameManager.gameManager._p2Health.Health);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Player2Heal(10);
            Debug.Log("player2: " + GameManager.gameManager._p2Health.Health);
        }

        //if player reaches 0 health, call Die()
        if(p2Health.Health <= 0)
        {
            Die();
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

    public void Player2Dmg(int dmg)
    {
        p2Health.dmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._p2Health.Health);
        Debug.Log("p2 health: " + GameManager.gameManager._p2Health.Health);    
    }

    public void Player2Heal(int heal)
    {
        p2Health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p2Health.Health);
    }

    void Die()
    {
        Debug.Log("p2 killed");
        p2controls.GetComponent<Rigidbody2D>().isKinematic = true;
        p2controls.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        anim.SetBool("IsKO", true);
        p2controls.p2CanMove = false;
        p2controls.canCrouch = false;
        p2combat.p2CanAttack = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameManager.gameManager.endRound("player");
        this.enabled = false;
    }

}
