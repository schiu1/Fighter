using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxAligner : MonoBehaviour
{
    //formula for changing hitbox is -[x value] + differential
    //for bridget it is .18
    //check if player1 or player2 
    //make it a method and call method in PlayerAssign
    [SerializeField]
    float differential = 0;

    public void FlipHitboxes()
    {
        foreach( Transform child in gameObject.transform)
        {
            if(child.tag == "Hitbox")
            {
                child.localPosition = new Vector3(-child.localPosition.x + differential, child.position.y, 0);
            }
        }
    }
}
