using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ignoredPlayer;

    void Start()
    {
        //make it on spawn ignore the collision of the loser
        //determine if the ignored player is p1 or p2 based on health
        //find the ignored player and set variable to them
        //call Physics2D.IgnoreCollision on their capsulecollider    
        if (GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health)
        {
            ignoredPlayer = GameObject.Find("Player2");
        }
        else if (GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health)
        {
            ignoredPlayer = GameObject.Find("Player1");
        }
        Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), ignoredPlayer.GetComponent<CapsuleCollider2D>());

        //ignore collision of right and left walls too
        GameObject rightWall = GameObject.Find("RightWall");
        GameObject leftWall = GameObject.Find("LeftWall"); ;
        Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), rightWall.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), leftWall.GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        //moves pirate constantly to the player
        transform.Translate(new Vector2(-1.5f, 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if((GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health) 
            && collision.gameObject.tag == "Player1")
        {
            Destroy(gameObject);
            GameObject p1 = GameObject.Find("Player1");
            //p1.GetComponent<P1Controls>().WinAnimSpin();
        }

        else if ((GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health)
            && collision.gameObject.tag == "Player2")
        {
            Destroy(gameObject);
            GameObject p2 = GameObject.Find("Player2");
            p2.GetComponent<P2Controls>().WinAnimSpin();
        }
    }
}
