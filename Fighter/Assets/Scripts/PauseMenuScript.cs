using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject firstSelected = null;

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

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.gameManager.isPaused = false;
        ToggleMenuActive();
    }
}
