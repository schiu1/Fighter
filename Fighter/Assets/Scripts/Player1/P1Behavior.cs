using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Behavior : MonoBehaviour
{
    Animator anim;
    P1Controls p1controls;
    P1Combat p1combat;
    UnitHealth p1Health;
    [SerializeField] Healthbar _healthbar = null;
    // Start is called before the first frame update
    void Start()
    {
        p1Health = GameManager.gameManager._p1Health;
        anim = gameObject.GetComponent<Animator>();
        p1controls = gameObject.GetComponent<P1Controls>();
        p1combat = gameObject.GetComponent<P1Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player1Dmg(10);
            
            Debug.Log("player1: " + GameManager.gameManager._p1Health.Health);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Player1Heal(10);
            Debug.Log("player1: " + GameManager.gameManager._p1Health.Health);
        }

        if(p1Health.Health <= 0)
        {
            Die();
        }
    }

    public void Player1Dmg(int dmg)
    {
        anim.SetTrigger("Flinch");
        p1Health.dmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
    }

    public void Player1Heal(int heal)
    {
        p1Health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
    }

    void Die()
    {
        Debug.Log("p2 killed");
        anim.SetBool("IsKO", true);
        p1controls.p1CanMove = false;
        p1controls.canCrouch = false;
        p1combat.p1CanAttack = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameManager.gameManager.endRound();
        this.enabled = false;
    }
}
