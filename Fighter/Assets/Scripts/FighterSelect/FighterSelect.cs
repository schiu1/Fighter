using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSelect : MonoBehaviour
{
    [SerializeField]
    GameObject[] fighters = null;
    public int currentFighter;

    void Start()
    {
        currentFighter = 0;
        fighters[currentFighter].SetActive(true);
    }

    public void Left()
    {
        if(currentFighter > 0)
        {
            fighters[currentFighter].SetActive(false);
            currentFighter -= 1;
            fighters[currentFighter].SetActive(true);
        }
    }

    public void Right()
    {
        if (currentFighter < fighters.Length - 1)
        {
            fighters[currentFighter].SetActive(false);
            currentFighter += 1;
            fighters[currentFighter].SetActive(true);
        }
    }

    /* part 1
     * this will be a template of a fighter
     * also a place to outline how to go about creating this
     * 
     * the main menu will go into Fighter_Select 
     * and show all characters that are available
     * there will be a next and previous button with
     * the fighter in idle stance in the middle
     * 
     * when the character is chosen and play button is pressed
     * the controls/behavior/combat scripts will be "filled in"
     * it will be filled in with the fields in this template
     * 
     * at first thought, it seems like majority of the difference
     * in fighters would be in: animations, 
     * floats that would sync up things with the animations of the fighter
     * like attack cooldowns and capCollider size,
     * and hitboxes in the inspector
     * 
     * so maybe this fighter script will hold access to the animations?
     * i don't know how to do that in c# so i would have to learn how to do that
     * it would also hold the values of floats that are character specific like
     * attack cooldowns and capCollider size to be inserted into controls/behavior/combat scripts
     * it would also maybe create the hurtboxes and hitboxes needed for character attacks
     * 
     * this looks intimidating
     * 
     * part 2
     * to load in gameobject of the fighter after choosing, i could make the fighters a prefab
     * it would hold basically everything about using the character, 
     * controls/behavior/combat would be attached
     * hitboxes would be attached
     * sprite and animations would be attached
     * 
     * take prefabs of all the fighters and store into an array in FighterSelect
     * representing the fighter as a value in an array and saving an int of what fighter was chosen will work
     * PlayerPrefs will help achieve this
     * 
     * part 3
     * encountered an issue with how the playerscripts will be able to interact with GameManager with health
     * i decided to make player1 and player2 versions of each character, ex: MayP1 and MayP2
     * so that there's no need to change the health names in GameManager based on which character is chosen
     * and just let character selection be in charge of choosing the player1 version for left and player2 for right
     * 
     * in lines of code that use "Player1" or "Player2" explicitly, i might use GameManager as a way to hold the names 
     * and GameManager can grab the names of the two players from PlayerPrefs
     */
}
