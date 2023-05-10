using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FighterAssign : MonoBehaviour
{
    [SerializeField]
    GameObject[] fighters = null;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(GameObject fighter in fighters)
        {
            var chosenPlayer1 = fighters[PlayerPrefs.GetInt("player1")].name;
            var chosenPlayer2 = fighters[PlayerPrefs.GetInt("player2")].name;
            var targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
     
            if (fighter.name == chosenPlayer1 && gameObject.name == "Player1")
            {
                var c = Instantiate(fighters[PlayerPrefs.GetInt("player1")], new Vector3(-3, 0, 0), Quaternion.identity, gameObject.transform); //spawn fighter
                c.GetComponent<SpriteRenderer>().flipX = true; //make them face enemy
                c.GetComponent<PlayerCombat>().SetEnemyLayers(LayerMask.NameToLayer("Player2")); //set enemylayers
                c.layer = LayerMask.NameToLayer("Player1"); //set own layer
                c.tag = "Player1"; //change Tag

                Cinemachine.CinemachineTargetGroup.Target target; //assign to TargetGroup1 for vcam
                target.target = c.transform;
                target.weight = 1;
                target.radius = 0;
                for (int i = 0; i < targetGroup.m_Targets.Length; i++)
                {
                    if(targetGroup.m_Targets[i].target == null)
                    {
                        targetGroup.m_Targets.SetValue(target, i);
                        break;
                    }
                }
                break;
            }
            else if(fighter.name == chosenPlayer2 && gameObject.name == "Player2")
            {
                var c = Instantiate(fighters[PlayerPrefs.GetInt("player2")], new Vector3(3, 0, 0), Quaternion.identity, gameObject.transform); //spawn fighter
                c.GetComponent<SpriteRenderer>().flipX = false; //make them face enemy
                c.GetComponent<PlayerCombat>().SetEnemyLayers(LayerMask.NameToLayer("Player1")); //set enemylayers
                c.layer = LayerMask.NameToLayer("Player2"); //set own layer
                c.tag = "Player2"; //change Tag

                Cinemachine.CinemachineTargetGroup.Target target; //assign to TargetGroup1 for vcam
                target.target = c.transform;
                target.weight = 1;
                target.radius = 0;
                for (int i = 0; i < targetGroup.m_Targets.Length; i++)
                {
                    if (targetGroup.m_Targets[i].target == null)
                    {
                        targetGroup.m_Targets.SetValue(target, i);
                        break;
                    }
                }
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * maybe use int to set to PlayerPref instead of string
     * have a public list of character names in GameManager or something (might not need this)
     * in the same order as the FighterList 1 and 2 in FighterSelection screen
     * and also have a list of GameObjects in this script in the same order as FighterList
     * then when this script getsint, it will use it as the index to the list of GameObjects
     * and instantiate the players from that GameObject list
     */
}
