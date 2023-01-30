using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject firstSelected = null;
    [SerializeField]
    GameObject previousMenu = null;

    public void ToggleOptionsActive()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
            if(EventSystem.current != null && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }

            if(SceneManager.GetSceneByName("Fight_Scene") == SceneManager.GetActiveScene())
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = !Cursor.visible;
            }
            //GameObject.Find("VolumeValue").GetComponent<Text>().text = SystemSettings.systemSettings.masterVolume.ToString("0.##");
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                GameObject.Find("VolumeValue").GetComponent<Text>().text = PlayerPrefs.GetFloat("masterVolume").ToString("0.##");
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(!gameObject.activeInHierarchy);

            if (SceneManager.GetSceneByName("Fight_Scene") == SceneManager.GetActiveScene())
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = !Cursor.visible;
            }
        }
    }

    public void OptionsBack()
    {
        if (SceneManager.GetSceneByName("Fight_Scene") == SceneManager.GetActiveScene())
        {
            previousMenu.GetComponent<PauseMenuScript>().InOptions = false;
            ToggleOptionsActive();
            previousMenu.GetComponent<PauseMenuScript>().ToggleMenuActive();
        }
        else
        {
            ToggleOptionsActive();
            previousMenu.GetComponent<MainMenuScript>().ToggleMainActive();
        }

    }
}
