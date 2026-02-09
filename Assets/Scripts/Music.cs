using UnityEngine;

public class Music : MonoBehaviour
{
    public MusicManager musicManager;
    public int trackNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicManager.PlayRandom();
        
    }
    void Update()
    {
        if (musicManager.playing == false){
            trackNumber += 1;
            if (musicManager.sounds[trackNumber].source != null)
            musicManager.sounds[trackNumber].source.Play();
            else
            trackNumber = 0;
        }
        
    }
}
