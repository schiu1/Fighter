﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Behavior : MonoBehaviour
{
    UnitHealth p2Health;
    [SerializeField] Healthbar _healthbar = null;
    // Start is called before the first frame update
    void Start()
    {
        p2Health = GameManager.gameManager._p2Health;
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
    }

    public void Player2Dmg(int dmg)
    {
        p2Health.dmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._p2Health.Health);
    }

    public void Player2Heal(int heal)
    {
        p2Health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p2Health.Health);
    }
}
