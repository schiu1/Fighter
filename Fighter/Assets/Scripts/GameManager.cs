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

    public int p1Score = 0;
    public int p2Score = 0;


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

    //keep track of healthbar in GameManager and Canvas
    //if there is winner, increment score in GameManager
    //Canvas will be checking what changed in GameManager when health becomes 0 and replicate that
    //Canvas will display either round win banner for 5 seconds or game win banner until user responds
    //if round win, GameManager will wait 5 sec, then reset scene
    //if game win, GameManager will wait for user input

    void Update()
    {
        if (_p1Health.Health <= 0)
        {
            p2Score += 1;
            //determine if roundwin or gamewin and call endround with that as arguement
        }
        else if(_p2Health.Health <= 0)
        {
            p2Score += 1;
        }

    }

    
}
