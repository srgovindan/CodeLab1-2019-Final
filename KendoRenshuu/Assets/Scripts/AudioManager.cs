using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;
    public List<AudioClip> audioClips;

    void Awake()
    {
        // SINGLETON
        if (AM == null)
        {
            DontDestroyOnLoad(gameObject);
            AM = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayAudioClip(int i, float volume=1f)
    {
        //Debug.Log("Playing Audio Clip " + i);
        GetComponent<AudioSource>().PlayOneShot(audioClips[i],volume);
    }
}
