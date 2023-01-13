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

    public int p1Wins = 0;
    public int p2Wins = 0;

    public int round = 1;

    public bool timedOut = false;

    public bool isPaused = false;

    SceneLoaderScript sceneLoader;
    public bool getSceneLoader;

    void Awake()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
        getSceneLoader = false;

        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote) && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            isPaused = false;
        }

        if (getSceneLoader)
        {
            sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
            getSceneLoader = false;
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

        if (p1Score == 2)
        {
            p1Wins += 1;
            StartCoroutine(Reset("game"));
        }
        else if (p2Score == 2)
        {
            p2Wins += 1;
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

            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 1f;
            
            //extend time before this happens later to show sprite win/lose animations
            bool done = false;
            while (!done)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    timedOut = false;
                    Scene scene = SceneManager.GetActiveScene();
                    sceneLoader.LoadLevel(scene.name);
                    yield return new WaitForSeconds(1);
                    _p1Health.Health = _p1Health.MaxHealth;
                    _p2Health.Health = _p2Health.MaxHealth;
                    p1Score = 0;
                    p2Score = 0;
                    round = 1;
                    done = true;
                }
                yield return null;
            }
        }
        else if(type == "round")
        {
            Debug.Log("round end");

            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 1f;

            yield return new WaitForSeconds(6f);
            _p1Health.Health = _p1Health.MaxHealth;
            _p2Health.Health = _p2Health.MaxHealth;
            round += 1;
            timedOut = false;
            Scene scene = SceneManager.GetActiveScene();
            sceneLoader.LoadLevel(scene.name);
        }
    }
}
