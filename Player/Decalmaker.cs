using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decalmaker : MonoBehaviour
{
    [SerializeField] Material Mat;
    Texture2D texture;

    void LateUpdate(){
        if(Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("taking screenshot");
            Screenshot();
        }
    }

    void Screenshot(){
        //yield new WaitForEndOfFrame();
        Debug.Log("taking screenshot");
        texture = ScreenCapture.CaptureScreenshotAsTexture();
        Debug.Log("taking Screenshot");

        Mat.SetTexture("MainTex", texture);
    }
}
