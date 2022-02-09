using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject Camera, mainMenu, settingsMenu;
    public float MoveSpeed;
    public Animator Animation;
    private Vector3 cameraStartPos, cameraEndPos, cameraStartRot, cameraEndRot;


    private void Awake(){
        cameraStartPos = new Vector3(0f, 0f, -4.5f);
        cameraEndPos = new Vector3(-4.5f, 0f, 0f);
        cameraStartRot = new Vector3(0f, 0f, 0f);
        cameraEndRot = new Vector3(0f, 90f, 0f);
    }

    public void Play(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options(){
        
        Animation.Play("toSettings");
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        //StartCoroutine(MoveToOptions());
    }

    public void Quit(){
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
