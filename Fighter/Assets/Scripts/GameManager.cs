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

    public int round = 1;

    public bool timedOut = false;

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

    public void endRound(string endType)
    {
        if(endType == "player")
        {
            if (_p1Health.Health <= 0)
            {
                p2Score += 1;
            
            }
            else if (_p2Health.Health <= 0)
            {
                p1Score += 1;
            }
        }
        else if (endType == "timer")
        {
            timedOut = true;
            if(_p1Health.Health > _p2Health.Health)
            {
                p1Score += 1;
            }
            else if (_p2Health.Health > _p1Health.Health)
            {
                p2Score += 1;
            }
        }

        if (p1Score == 2 || p2Score == 2)
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
            //extend time before this happens later to show sprite win/lose animations
            bool done = false;
            while (!done)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    _p1Health.Health = _p1Health.MaxHealth;
                    _p2Health.Health = _p2Health.MaxHealth;
                    p1Score = 0;
                    p2Score = 0;
                    round = 1;
                    timedOut = false;
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                    done = true;
                }
                yield return null;
            }
        }
        else if(type == "round")
        {
            Debug.Log("round end");
            yield return new WaitForSeconds(3);
            _p1Health.Health = _p1Health.MaxHealth;
            _p2Health.Health = _p2Health.MaxHealth;
            round += 1;
            timedOut = false;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
