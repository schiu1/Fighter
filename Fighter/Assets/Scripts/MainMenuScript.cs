using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    SceneLoaderScript crossfade;
    void Start()
    {
        crossfade = GameObject.Find("SceneLoader").GetComponent<SceneLoaderScript>();
    }

    public void StartGame()
    {
        crossfade.LoadLevel("Fight_Scene");
    }



    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
