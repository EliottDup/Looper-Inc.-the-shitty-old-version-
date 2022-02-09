using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeManager : MonoBehaviour
{
    public float time;
    void Awake(){
        Time.timeScale = time;
    }
}
