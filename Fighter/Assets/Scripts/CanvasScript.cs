using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    Text p1ScoreUI;
    Text p2ScoreUI;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gameManager;
        p1ScoreUI = transform.Find("P1_Score").GetComponent<Text>();
        p2ScoreUI = transform.Find("P2_Score").GetComponent<Text>();
        p1ScoreUI.text = gm.p1Score.ToString();
        p2ScoreUI.text = gm.p2Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
