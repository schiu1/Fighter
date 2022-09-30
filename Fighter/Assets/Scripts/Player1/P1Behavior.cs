using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Behavior : MonoBehaviour
{
    UnitHealth p1Health;
    [SerializeField] Healthbar _healthbar = null;
    // Start is called before the first frame update
    void Start()
    {
        p1Health = GameManager.gameManager._p1Health;
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
    }

    void Player1Dmg(int dmg)
    {
        p1Health.dmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
    }

    void Player1Heal(int heal)
    {
        p1Health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
    }
}
