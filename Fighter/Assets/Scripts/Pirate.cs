using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //make it on spawn ignore the collision of the loser
        //determine if the ignored player is p1 or p2 based on health
        //find the ignored player and set variable to them
        //call Physics2D.IgnoreCollision on their capsulecollider    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(-1.5f, 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if((GameManager.gameManager._p1Health.Health > GameManager.gameManager._p2Health.Health) 
            && collision.gameObject.tag == "Player1")
        {
            Destroy(gameObject);
        }

        else if ((GameManager.gameManager._p1Health.Health < GameManager.gameManager._p2Health.Health)
            && collision.gameObject.tag == "Player2")
        {
            Destroy(gameObject);
        }
    }
}
