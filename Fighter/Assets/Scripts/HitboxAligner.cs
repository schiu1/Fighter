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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
