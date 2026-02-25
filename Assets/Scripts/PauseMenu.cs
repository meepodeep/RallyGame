using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    InputAction pauseInput;
    InputAction backInput;


    bool isPaused = false;
    public GameObject pauseMenu;
    public AudioMixer master;
    public GameObject UIMenu;
    void Start()
    {
        pauseInput = InputSystem.actions.FindAction("Pause");
        backInput = InputSystem.actions.FindAction("Back");
    }
    void Update()
    {
        if (pauseInput.WasPressedThisFrame() && isPaused == false)
        {
            isPaused = true;
            pauseMenu.SetActive(isPaused);
            UIMenu.SetActive(!isPaused);
            Pause();
        }
        else if (pauseInput.WasPressedThisFrame()||backInput.WasPressedThisFrame())
        {
            isPaused = false;
            pauseMenu.SetActive(isPaused);
            UIMenu.SetActive(!isPaused);
            Resume();
        }

        
    }
    void Pause()
    {
        Time.timeScale = 0;
        master.SetFloat ("MasterVolume", -40);
    }
    void Resume()
    {
        master.SetFloat ("MasterVolume", -10);
        Time.timeScale = 1;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is createddate is called once per fram
    public void Restart()
    {
        // Reload the current scene by name
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
        Resume();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
