using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    protected Animator anim;
    //this gameobject's playercontrols variable init here
    protected float lastAttack = 0f;
    protected float attackCD = 0f;
    //HideInInspector public bool playerAttacking variable init here
    //- - - playerCanAttack - - -
    public bool attacking;
    public bool canAttack;

    [Header("General")]
    [SerializeField] protected LayerMask enemyLayers = 0;

    [Header("Punch")]
    [SerializeField] protected Transform punchAttackPoint = null;
    [SerializeField] protected Vector2 punchAttackRange = Vector2.zero;

    [Header("Crouch Punch")]
    [SerializeField] protected Transform cPunchAttackPoint = null;
    [SerializeField] protected Vector2 cPunchAttackRange = Vector2.zero;

    [Header("Slash")]
    [SerializeField] protected Transform slashAttackPoint = null;
    [SerializeField] protected Vector2 slashAttackRange = Vector2.zero;

    [Header("Heavy Slash")]
    [SerializeField] protected Transform heavyAttackPoint = null;
    [SerializeField] protected Vector2 heavyAttackRange = Vector2.zero;

    [Header("Kick")]
    [SerializeField] protected Transform kickAttackPoint = null;
    [SerializeField] protected Vector2 kickAttackRange = Vector2.zero;

    [Header("Visual Effects")]
    [SerializeField]
    protected GameObject slashEffect = null;
    [SerializeField]
    protected GameObject punchEffect = null;

    protected void Awake()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Fight_Scene"))
        {
            this.enabled = false;
        }
    }

    //void Start() goes here

    //void Update() goes here

    protected IEnumerator Hitstop(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    //void punch/kick/slash/heavyslash goes here

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(punchAttackPoint.position, punchAttackRange);
        Gizmos.DrawWireCube(cPunchAttackPoint.position, cPunchAttackRange);
        Gizmos.DrawWireCube(slashAttackPoint.position, slashAttackRange);
        Gizmos.DrawWireCube(heavyAttackPoint.position, heavyAttackRange);
        Gizmos.DrawWireCube(kickAttackPoint.position, kickAttackRange);
    }
}
