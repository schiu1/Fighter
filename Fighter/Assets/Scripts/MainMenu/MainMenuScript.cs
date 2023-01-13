using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    SceneLoaderScript crossfade;
    [SerializeField]
    GameObject optionsMenu = null;
    [SerializeField]
    GameObject firstSelected = null;
    void Start()
    {
        crossfade = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
    }

    public void ToggleMainActive()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
            if (EventSystem.current != null && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }

    public void StartGame()
    {
        crossfade.LoadLevel("Fight_Scene");
    }

    public void OptionsMenu()
    {
        //turn off main menu
        ToggleMainActive();
        //open options menu
        optionsMenu.GetComponent<OptionsMenuScript>().ToggleOptionsActive();
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
