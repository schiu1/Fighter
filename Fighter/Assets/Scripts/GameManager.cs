using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //this is what makes a class a Singleton
    public static GameManager gameManager { get; private set; }

    //player1 and player2
    public UnitHealth _p1Health = new UnitHealth(100, 100);
    public UnitHealth _p2Health = new UnitHealth(100, 100);

    void Awake()
    {
        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
