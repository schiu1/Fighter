using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    protected Animator anim;
    //p1controls here
    //p1combat here
    //playerhealth here
    [SerializeField] protected Healthbar _healthbar = null;

    protected float startTime;
    protected bool started;

    [SerializeField]
    protected GameObject hitEffect = null;

    protected GameObject vcam;
    protected Shake shake;

    //Awake() goes here

    //Start() goes here

    //Update goes here

    public virtual void PlayerDmg(int dmg)
    {

    }

    public virtual void PlayerHeal(int heal)
    {

    }

    protected virtual void Die()
    {

    }
}
