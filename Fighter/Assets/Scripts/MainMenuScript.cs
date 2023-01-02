using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    Animator crossfade = null;

    public void StartGame()
    {
        StartCoroutine(Crossfade());  
    }

    IEnumerator Crossfade()
    {
        crossfade.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Fight_Scene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
