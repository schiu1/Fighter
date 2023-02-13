using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgetP1Combat : PlayerCombat
{
    BridgetP1Controls p1Controls;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        p1Controls = gameObject.GetComponent<BridgetP1Controls>();

        attacking = false;
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
