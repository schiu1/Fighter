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
            ignoredPlayer = GameObject.Find(GameManager.gameManager.p2Name + "(Clone)");
            Debug.Log(ignoredPlayer.name);
        }
        else if (GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health)
        {
            ignoredPlayer = GameObject.Find(GameManager.gameManager.p1Name + "(Clone)");
            Debug.Log(ignoredPlayer.name);
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
            Debug.Log("pirate touched p1");
            Destroy(gameObject);
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player1"))
            {
                if (obj.name == GameManager.gameManager.p1Name + "(Clone)")
                {
                    obj.GetComponent<MayControls>().WinAnimSpin();
                    break;
                }
            }
        }

        else if ((GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health)
            && collision.gameObject.tag == "Player2")
        {
            Debug.Log("pirate touched p2");
            Destroy(gameObject);
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player2"))
            {
                if(obj.name == GameManager.gameManager.p2Name + "(Clone)")
                {
                    obj.GetComponent<MayControls>().WinAnimSpin();
                    break;
                }
            }
        }
    }
}
