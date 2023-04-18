using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public Vector3 AttackPoint { get; set; }
    public Vector2 AttackRange { get; set; }
    public int AttackDamage { get; set; }
    public string PushType { get; set; }
    public string SoundType { get; set; }
}
