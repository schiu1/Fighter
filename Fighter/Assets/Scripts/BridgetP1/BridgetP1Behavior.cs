using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgetP1Behavior : MonoBehaviour
{
    Animator anim;
    BridgetP1Controls p1controls;
    BridgetP1Combat p1combat;
    UnitHealth p1Health;
    [SerializeField] Healthbar _healthbar = null;

    float startTime;
    bool started;

    [SerializeField]
    GameObject hitEffect = null;

    GameObject vcam;
    Shake shake;

    void Awake()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Fight_Scene"))
        {
            this.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        p1Health = GameManager.gameManager._p1Health;
        anim = gameObject.GetComponent<Animator>();
        p1controls = gameObject.GetComponent<BridgetP1Controls>();
        p1combat = gameObject.GetComponent<BridgetP1Combat>();

        startTime = Time.time;
        started = false;

        vcam = GameObject.Find("CM vcam1");
        shake = vcam.GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameManager.isPaused)
        {
            if (Time.time - startTime >= 3f && !started)
            {
                started = true;
                p1controls.p1CanMove = true;
                p1controls.canCrouch = true;
                p1combat.p1CanAttack = true;
            }

            //leave out self-heal and self-dmg

            if(p1Health.Health <= 0)
            {
                Die();
            }
        }
    }

    public void Player1Dmg(int dmg)
    {
        p1Health.dmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
        GameObject b = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(b, .2f); //based on the particle system's duration
        shake.ShakeCamera(.3f, .5f);
    }

    public void Player1Heal(int heal)
    {
        p1Health.healUnit(heal);
        _healthbar.SetHealth(GameManager.gameManager._p1Health.Health);
    }

    void Die()
    {
        Debug.Log("p1 killed");
        anim.SetBool("IsKO", true);
        p1controls.p1CanMove = false;
        p1controls.canCrouch = false;
        p1combat.p1CanAttack = false;
        GameManager.gameManager.endRound("player");
        this.enabled = false;
    }
}
