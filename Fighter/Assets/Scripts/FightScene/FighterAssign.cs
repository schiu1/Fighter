using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAssign : MonoBehaviour
{
    [SerializeField]
    GameObject[] fighters = null;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(GameObject fighter in fighters)
        {
            if(fighter.name == PlayerPrefs.GetString("player1") && gameObject.name == "Player1")
            {
                fighter.SetActive(true);
                break;
            }
            else if(fighter.name == PlayerPrefs.GetString("player2") && gameObject.name == "Player2")
            {
                fighter.SetActive(true);
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
