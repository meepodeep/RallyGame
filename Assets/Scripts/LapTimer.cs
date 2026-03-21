using System;
using TMPro;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    [HideInInspector]
    public float lapTime = 0;
    [HideInInspector]
    public string lapTimeText;
    public TMP_Text timer;
    bool isTiming;
    private finishScreen finishScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishScreen = FindAnyObjectByType<finishScreen>();
    }
    // Update is called once per frame
    void Update()
    {
       if (isTiming){
            lapTime += Time.deltaTime;;
        }
       
       TimeSpan time = TimeSpan.FromSeconds(lapTime);
       lapTimeText = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
       timer.text = lapTimeText;
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
            finishScreen.Finish();
        }
    }
}
