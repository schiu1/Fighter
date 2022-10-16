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
    // Start is called before the first frame update
    void Start()
    {
        p2Health = GameManager.gameManager._p2Health;
        anim = gameObject.GetComponent<Animator>();
        p2controls = gameObject.GetComponent<P2Controls>();
        p2combat = gameObject.GetComponent<P2Combat>();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Player2Dmg(int dmg)
    {
        anim.SetTrigger("Flinch");
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
        anim.SetBool("IsKO", true);
        p2controls.p2CanMove = false;
        p2controls.canCrouch = false;
        p2combat.p2CanAttack = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameManager.gameManager.endRound();
        this.enabled = false;
    }

}
