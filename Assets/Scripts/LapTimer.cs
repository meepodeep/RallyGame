using System;
using TMPro;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    [HideInInspector]
    public float lapTime = 0;
    public TMP_Text timer;
    bool isTiming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
       if (isTiming){
            lapTime += Time.deltaTime;;
        }
       
       TimeSpan time = TimeSpan.FromSeconds(lapTime);
       timer.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Start")
        {
            isTiming = true;
        }
        if (other.tag == "Finish")
        {
            isTiming = false;
        }
    }
}
