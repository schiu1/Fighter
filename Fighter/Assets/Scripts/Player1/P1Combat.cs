using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Combat : MonoBehaviour
{
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("P1_Punch"))
        {
            //anim calls punch animation
            punch();
        }
        if (Input.GetButtonDown("P1_Kick"))
        {
            //anim calls kick animation
            kick();
        }
        if (Input.GetButtonDown("P1_Slash"))
        {
            //anim calls slash animation
            slash();
        }
        if (Input.GetButtonDown("P1_HeavySlash"))
        {
            //anim calls HS animation
            heavySlash();
        }
    }

    //put these methods in attack anim as events
    void punch()
    {

    }

    void kick()
    {

    }

    void slash()
    {

    }

    void heavySlash()
    {

    }
}
