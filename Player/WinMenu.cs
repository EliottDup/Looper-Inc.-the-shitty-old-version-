using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public WinCube WC;
    public GameObject WinMenuUI, GUI;
    private bool temp;
    void Awake()
    {
        WinMenuUI.SetActive(false);
    }

    void Update(){
        if (temp == WC.hasWon){
            return;
        }
        else{
            temp = WC.hasWon;
            Debug.Log("I win!");
            WinMenuUI.SetActive(true);
            GUI.SetActive(false);
            Time.timeScale = 0.1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MonoBehaviour[] scripts = PlayerManager.instance.player.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour script in scripts){
                script.enabled = false;
            }
        }
    }

    // Update is called once per frame
    public void NextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
