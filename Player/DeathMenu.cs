using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public bool IsDed = false;
    [SerializeField] GameObject DeathGUI, GUI;

    void Awake()
    {
    }

    void Update()
    {
        if (IsDed){
            DeathGUI.SetActive(true);
            GUI.SetActive(false);
            Time.timeScale = 0.1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MonoBehaviour[] scripts = PlayerManager.instance.player.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour script in scripts){
                script.enabled = false;
            }
            scripts = PlayerManager.instance.playerRev.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour script in scripts){
                script.enabled = false;
            }
        }    
    }
    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
