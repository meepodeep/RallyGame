using UnityEngine.Audio;
using System;
using UnityEngine;
using Unity.Mathematics;

public class EngineSoundManager : MonoBehaviour
{
    float isThrottle;
    public Sound[] sounds;
    public CarControl dz;
    bool EngineOn = true; 
    // Start is called before the first frame update
    void Awake ()
    {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.name = s.clip.name;

            s.source.spatialBlend = s.spatialBlend;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
           
        }
    }


    // Update is called once per frame
    public void Play (string name)
    {
        //determines what sound is being called//
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
        //if the name of the sound that is currently playing is equal to the string propSound then PropOn=true else it is false//
        if (name == "EngineNoise")
            {
                EngineOn = true; 
            }
            else 
            {
                EngineOn = false; 
            }
    }
    
    void Update()
    {     
        //if a sound is playing//
        foreach (Sound s in sounds)
        {
            //if PropOn is on//
            if (EngineOn == true)
            {
                s.source.pitch = Mathf.Pow(isThrottle*1.1f, 2f);
                s.source.volume = isThrottle-1;
            } else
            {
                s.source.pitch = s.pitch; 
                s.source.volume = s.volume;
            }
        }
        isThrottle = (dz.engineRpm/10000)+1;
        
    }
}
