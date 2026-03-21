using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class finishScreen : MonoBehaviour
{
    public Animator timer;
    public TMP_Text timerText;
    public Animator finish;
    private LapTimer lapTimer; 
    public GameObject background;
    private bool finished = false;
    public float nextScenetimer =30 ;
    public MusicManager musicManager;
    public GameObject uiCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Finish()
    {
        background.SetActive(true);
        lapTimer = FindAnyObjectByType<LapTimer>();
        musicManager = FindAnyObjectByType<MusicManager>();
        timerText.text = lapTimer.lapTimeText;
        timer.SetBool("TimerGO", true);
        finish.SetBool("FinishGO", true);
        finished = true;
    }
    void Update()
    {
        if (finished == true)
        {
            uiCanvas.SetActive(false);
            if(Time.timeScale >= .05f)
            {
                Time.timeScale -= .5f*Time.deltaTime;

            }
            else
            {
                Time.timeScale = 1;
                if(SceneManager.GetActiveScene().buildIndex <2){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    Destroy(musicManager.gameObject);
                    SceneManager.LoadScene(0);
                }
                
            }
            
        }
    }
}
