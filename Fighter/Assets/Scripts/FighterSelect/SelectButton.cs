using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    /*
     * P1Fighter and P2Fighter are the FighterSelection components and get them
     * have a single array, for copying p1 and p2 characters array in inspector
     * get currentFighter int in both p1 and p2
     * when Select button is pressed, take both the ints and input them into character array
     * get names of p1 and p2 fighters and assign them in PlayerPref, 
     * similar to select button in FighterSelect
     */
    FighterSelect p1Fighter;
    FighterSelect p2Fighter;
    [SerializeField]
    GameObject[] FighterList1 = null;
    [SerializeField]
    GameObject[] FighterList2 = null;
    SceneLoaderScript crossfade;

    // Start is called before the first frame update
    void Start()
    {
        p1Fighter = GameObject.Find("Fighters1").GetComponent<FighterSelect>();
        p2Fighter = GameObject.Find("Fighters2").GetComponent<FighterSelect>();
        crossfade = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
    }

    public void Select()
    {
        int p1Index = p1Fighter.currentFighter;
        int p2Index = p2Fighter.currentFighter;
        //SetInt of index here
        //PlayerPrefs.SetString("player1", FighterList1[p1Index].gameObject.name);
        //PlayerPrefs.SetString("player2", FighterList2[p2Index].gameObject.name);
        PlayerPrefs.SetInt("player1", p1Index);
        PlayerPrefs.SetInt("player2", p2Index);
        crossfade.LoadLevel("Fight_Scene");
    }
}
