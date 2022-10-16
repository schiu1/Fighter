using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    Text p1ScoreUI;
    Text p2ScoreUI;
    GameManager gm;
    GameObject roundBanner;
    GameObject gameBanner;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gameManager;
        p1ScoreUI = transform.Find("P1_Score").GetComponent<Text>();
        p2ScoreUI = transform.Find("P2_Score").GetComponent<Text>();
        roundBanner = transform.Find("RoundOver").gameObject;
        gameBanner = transform.Find("GameOver").gameObject;
        p1ScoreUI.text = gm.p1Score.ToString();
        p2ScoreUI.text = gm.p2Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.p1Score.ToString() != p1ScoreUI.text)
        {
            p1ScoreUI.text = gm.p1Score.ToString();
            ShowBanner();
        }
        else if (gm.p2Score.ToString() != p2ScoreUI.text)
        {
            p2ScoreUI.text = gm.p2Score.ToString();
            ShowBanner();
        }

        
    }

    void ShowBanner()
    {
        if (gm.p1Score == 2 || gm.p2Score == 2)
        {
            gameBanner.SetActive(true);
        }
        else
        {
            roundBanner.SetActive(true);
        }
    }
}
