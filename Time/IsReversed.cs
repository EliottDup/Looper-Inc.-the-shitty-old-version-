using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsReversed : MonoBehaviour
{
    public static IsReversed Instance {get; set;}

    public bool isReversed = false;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            isReversed = true;
        }
    }
}
