using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public class SettingsMenu : MonoBehaviour
{
    public GameObject Camera, mainMenu, settingsMenu;
    public TextMeshProUGUI volumeText;
    public AudioMixer audioMixer;
    public TMP_Dropdown resDropdown;
    public float MoveSpeed;
    public Animator Animation;
    private Vector3 cameraStartPos, cameraEndPos, cameraStartRot, cameraEndRot;
    Resolution[] resolutions;

    void Awake(){
        cameraStartPos = new Vector3(-4.5f, 0f, 0f);
        cameraEndPos = new Vector3(0f, 0f, -4.5f);
        cameraStartRot = new Vector3(0f, 90f, 0f);
        cameraEndRot = new Vector3(0f, 0f, 0f);

        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        int currentResIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    private void start(){
        
    }
    
    public void Back(){
        Animation.speed = 1/MoveSpeed;
        Animation.Play("Empty");
        Animation.Play("fromSettings");
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void setResolution(int ResIndex){
        Resolution resolution = resolutions[ResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setFullscreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }

    public void setQual(int qualIndex){
        QualitySettings.SetQualityLevel(qualIndex);
    }

    public void SetVolume (float volume){
        volumeText.text = (volume+80).ToString();
        audioMixer.SetFloat("Volume", volume);
    }
}
