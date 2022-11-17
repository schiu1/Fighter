using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector] //needs to be public so that AudioManager can access it
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
