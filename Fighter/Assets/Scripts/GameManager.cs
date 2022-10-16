using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //this is what makes a class a Singleton
    public static GameManager gameManager { get; private set; }

    //player1 and player2
    public UnitHealth _p1Health = new UnitHealth(100, 100);
    public UnitHealth _p2Health = new UnitHealth(100, 100);

    public int p1Score = 0;
    public int p2Score = 0;
    //maybe keep track of number of rounds here and CanvasScript will look at this field to say "Round #", wait 2 sec, "FIGHT"

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

    }

    public void endRound()
    {
        if (_p1Health.Health <= 0)
        {
            p2Score += 1;
            
        }
        else if (_p2Health.Health <= 0)
        {
            p1Score += 1;
        }

        if(p1Score == 2 || p2Score == 2)
        {
            StartCoroutine(Reset("game"));
        }
        else
        {
            StartCoroutine(Reset("round"));
        }
    }

    IEnumerator Reset(string type)
    {
        if(type == "game")
        {
            Debug.Log("game end");
            //wait for player input to restart
        }
        else if(type == "round")
        {
            Debug.Log("round end");
            yield return new WaitForSeconds(3);
            _p1Health.Health = _p1Health.MaxHealth;
            _p2Health.Health = _p2Health.MaxHealth;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
