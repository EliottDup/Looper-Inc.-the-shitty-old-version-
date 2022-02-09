using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject pauseMenuUI, GUI;
    
    void Awake(){
        GameIsPaused = false;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        GUI.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        GUI.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart(){
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    
}
