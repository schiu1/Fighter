using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxAligner : MonoBehaviour
{
    //formula for changing hitbox is -[x value] + differential
    //for bridget it is .18
    //check if player1 or player2 
    //make it a method and call method in PlayerAssign

    public void FlipHitboxes()
    {
        foreach( Transform child in gameObject.transform)
        {
            if(child.tag == "Hitbox")
            {
                child.localPosition = new Vector3(-child.localPosition.x, child.localPosition.y, 0);
            }
        }
        
        var hurtbox = gameObject.GetComponent<CapsuleCollider2D>();
        var groundCollider = gameObject.GetComponent<BoxCollider2D>();
        hurtbox.offset = new Vector2(-hurtbox.offset.x, hurtbox.offset.y);
        groundCollider.offset = new Vector3(-groundCollider.offset.x, groundCollider.offset.y);
        
    }
}
