using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    Animator crossfade;

    void Awake()
    {
        crossfade = GameObject.Find("Crossfade").GetComponent<Animator>();
    }

    public void LoadLevel(string scene)
    {
        StartCoroutine(Crossfade(scene));
    }

    IEnumerator Crossfade(string sceneName)
    {
        crossfade.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
