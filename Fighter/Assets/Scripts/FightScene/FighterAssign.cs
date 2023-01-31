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
}
