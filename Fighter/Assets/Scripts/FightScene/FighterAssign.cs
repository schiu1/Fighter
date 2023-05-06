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
            var chosenPlayer1 = fighters[PlayerPrefs.GetInt("player1")].name;
            var chosenPlayer2 = fighters[PlayerPrefs.GetInt("player2")].name;

            if (fighter.name == chosenPlayer1 && gameObject.name == "Player1")
            {
                //fighter.SetActive(true);
                Instantiate(fighters[PlayerPrefs.GetInt("player1")], new Vector3(-3, 0, 0), Quaternion.identity, gameObject.transform);
                break;
            }
            else if(fighter.name == chosenPlayer2 && gameObject.name == "Player2")
            {
                //fighter.SetActive(true);
                var c = Instantiate(fighters[PlayerPrefs.GetInt("player2")], new Vector3(3, 0, 0), Quaternion.identity, gameObject.transform);
                c.GetComponent<SpriteRenderer>().flipX = false;
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
