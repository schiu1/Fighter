using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    Text p1ScoreUI;
    Text p2ScoreUI;

    Text p1WinsUI;
    Text p2WinsUI;

    GameManager gm;

    Text roundStartNumber;
    GameObject fightBanner;

    GameObject roundEndBanner;
    GameObject gameBanner;

    float startTime;
    bool started;

    Text timerUI;
    [SerializeField] float currentTime;
    bool timerStarted;

    PauseMenuScript pauseMenu;
    OptionsMenuScript optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gameManager;

        p1ScoreUI = transform.Find("P1_Score").GetComponent<Text>();
        p2ScoreUI = transform.Find("P2_Score").GetComponent<Text>();
        p1ScoreUI.text = gm.p1Score.ToString();
        p2ScoreUI.text = gm.p2Score.ToString();

        p1WinsUI = transform.Find("P1_Wins").GetComponent<Text>();
        p2WinsUI = transform.Find("P2_Wins").GetComponent<Text>();
        p1WinsUI.text = gm.p1Wins.ToString();
        p2WinsUI.text = gm.p2Wins.ToString();

        roundEndBanner = transform.Find("RoundOver").gameObject;
        gameBanner = transform.Find("GameOver").gameObject;

        roundStartNumber = transform.Find("RoundStart").GetComponent<Text>();
        roundStartNumber.text = "ROUND " + gm.round.ToString();

        fightBanner = transform.Find("Fight").gameObject;

        //currentTime = 10f;
        timerUI = transform.Find("Timer").GetComponent<Text>();
        timerUI.text = currentTime.ToString("f1");
        timerStarted = false;

        startTime = Time.time;
        started = false;

        pauseMenu = transform.Find("PauseMenu").GetComponent<PauseMenuScript>();
        optionsMenu = transform.Find("OptionsMenu").GetComponent<OptionsMenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.isPaused && !pauseMenu.gameObject.activeSelf && !pauseMenu.InOptions)
        {
            pauseMenu.ToggleMenuActive();
        }
        else if (!GameManager.gameManager.isPaused && pauseMenu.gameObject.activeSelf)
        {
            if (!pauseMenu.InOptions)
            {
                pauseMenu.ToggleMenuActive();
            }
            else
            {
                Debug.Log("in here");
                optionsMenu.ToggleOptionsActive();
            }
        }

        if(Time.time - startTime >= 3f && !started)
        {
            started = true;
            timerStarted = true;
            StartCoroutine(StartBanners());
        }

        if (timerStarted)
        {
            currentTime -= Time.deltaTime;
            timerUI.text = currentTime.ToString("f1");
            if (currentTime <= 0)
            {
                timerStarted = false;
                currentTime = 0;
                timerUI.text = currentTime.ToString();
                GameManager.gameManager.endRound("timer");
            }
        }

        if (gm.p1Score.ToString() != p1ScoreUI.text)
        {
            timerStarted = false;
            p1ScoreUI.text = gm.p1Score.ToString();
            p1WinsUI.text = gm.p1Wins.ToString();
            ShowBanner();
        }
        else if (gm.p2Score.ToString() != p2ScoreUI.text)
        {
            timerStarted = false;
            p2ScoreUI.text = gm.p2Score.ToString();
            p2WinsUI.text = gm.p2Wins.ToString();
            ShowBanner();
        }
        else if (currentTime == 0 &&  gm._p1Health.Health == gm._p2Health.Health)
        {
            ShowBanner();
        }
    }

    IEnumerator StartBanners()
    {
        roundStartNumber.gameObject.SetActive(false);
        fightBanner.SetActive(true);
        yield return new WaitForSeconds(1);
        fightBanner.SetActive(false);
    }

    void ShowBanner()
    {
        if (gm.p1Score == 2 || gm.p2Score == 2)
        {
            gameBanner.SetActive(true);
        }
        else
        {
            roundEndBanner.SetActive(true);
        }
    }
}
