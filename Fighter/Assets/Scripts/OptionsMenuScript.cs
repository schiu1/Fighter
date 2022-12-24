using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject firstSelected = null;
    [SerializeField]
    GameObject pauseMenu = null;

    public void ToggleOptionsActive()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
            if(EventSystem.current != null && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(!gameObject.activeInHierarchy);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }

    public void OptionsBack()
    {
        pauseMenu.GetComponent<PauseMenuScript>().InOptions = false;
        ToggleOptionsActive();
        pauseMenu.GetComponent<PauseMenuScript>().ToggleMenuActive();

    }
}
