using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //make this a singleton so that Theme is persistent in Start()

    public static AudioManager audioManager;

    [SerializeField]
    Sound[] sounds = null;
    
    void Awake()
    {
        //check to see if original AudioManager
        if(audioManager != null && audioManager != this)
        {
            Destroy(this);
        }
        else
        {
            audioManager = this;
            DontDestroyOnLoad(audioManager);
        }

        //for each sound in array, 
        foreach (Sound s in sounds)
        {
            //assign the AudioSource variable in Sound an instance of AudioSource
            //and assign the properties saved in Sound obj to the AudioSource obj
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.name = s.soundName;
            s.source.volume = s.volume;
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        PlaySound("Theme");
    }

    public void PlaySound(string name)
    {
        Sound found = null;
        foreach (Sound s in sounds)
        {
            if (s.soundName == name)
            {
                found = s;
            }
        }
        if(found == null)
        {
            Debug.Log("Sound with " + name + " was not found");
            return;
        }
        found.source.Play();
    }
    //when calling to play a sound clip, it gets it from an AudioSource component of same name
    //so instead of creating AudioSource components manually, spamming the gameobject, we use this to automate it
    //this script will automatically create an AudioSource in the same gameobject for each Sound type object in the Sound[] clips array

    //the Sound class will have the fields: string name, AudioClip clip, float volume, bool loop, and AudioSource source
    //the reason AudioSource is there is to store it so that when we play it later, we can call the Play() method on the AudioSource within Sound object
    //(it also makes code look cleaner as opposed to creating another array in AudioManager that represent each AudioSource)

    //the Sound[] clips array will be populated through the inspector, so it should be public/private with SerializeField
    //in Awake(), that should be where each Sound type clip will be inserted into a new AudioSource component for each clip
    //use for each loop, and include the same fields
    //create Play() method within AudioManager too, with string name as parameter
}
