using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    bool fightSceneLoaded;
    void Awake()
    {
        fightSceneLoaded = false;
    }

    public void StartGame()
    {
        LoadFightScene();
        if (fightSceneLoaded)
        {
            Debug.Log(SceneManager.GetSceneByName("Fight_Scene").name);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Fight_Scene"));
        }
    }

    void LoadFightScene()
    {
        SceneManager.LoadScene("Fight_Scene");
        fightSceneLoaded = !fightSceneLoaded;
    }
}
