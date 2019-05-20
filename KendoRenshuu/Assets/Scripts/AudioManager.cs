using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;

    public Dictionary<string, AudioClip> AudioClipLibrary;
    public List<string> AudioClipNames;
    public List<AudioClip> AudioClips;

    private void Awake()
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

        //BUILD THE AUDIO LIBRARY DICTIONARY
        AudioClipLibrary = new Dictionary<string, AudioClip>(); //init the dictionary
        //build the Dictionary from the lists in the inspector
        for (var i = 0; i < AudioClipNames.Count; i++) AudioClipLibrary.Add(AudioClipNames[i], AudioClips[i]);
    }

    public void PlayClipName(string clipname, float volume = 1f)
    {
        AudioClip clip; //init an empty audio clip
        AudioClipLibrary.TryGetValue(clipname, out clip); //look in the dictionary for the clip with clipname
        if (clip != null) //if the clip exists in the dictionary
            GetComponent<AudioSource>().PlayOneShot(clip, volume); //play the clip 
    }

    public void PlayAudioClip(int i, float volume = 1f)
    {
        //Debug.Log("Playing Audio Clip " + i);
        GetComponent<AudioSource>().PlayOneShot(AudioClips[i], volume);
    }
}