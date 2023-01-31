using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject firstSelected = null;
    [HideInInspector]
    public bool InOptions = false;
    /*
    [SerializeField]
    GameObject firstOptionSelected = null;
    */
    [SerializeField]
    GameObject optionsMenu = null;

    public void ToggleMenuActive()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
            if(EventSystem.current != null && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(!gameObject.activeInHierarchy);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /*
    public void ToggleOptionsActive()
    {
        if (!optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
            if (EventSystem.current != null && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstOptionSelected);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
    */

    public void Options()
    {
        InOptions = true; //test
        ToggleMenuActive();
        optionsMenu.GetComponent<OptionsMenuScript>().ToggleOptionsActive();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.gameManager.isPaused = false;
        ToggleMenuActive();
    }


    public void Quit()
    {
        SceneManager.MoveGameObjectToScene(AudioManager.audioManager.gameObject, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(GameManager.gameManager.gameObject, SceneManager.GetActiveScene());
        Time.timeScale = 1;
        SceneLoaderScript sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
        sceneLoader.LoadLevel("Main_Menu");
        
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
