using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    SceneLoaderScript crossfade;
    [SerializeField]
    GameObject optionsMenu;
    void Start()
    {
        crossfade = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
    }

    public void StartGame()
    {
        crossfade.LoadLevel("Fight_Scene");
    }

    public void OptionsMenu()
    {
        //turn off main menu and logo
        GameObject.Find("MainMenu").SetActive(false);
        GameObject.Find("Logo").SetActive(false);
        //open options menu
        optionsMenu.GetComponent<OptionsMenuScript>().ToggleOptionsActive();
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
