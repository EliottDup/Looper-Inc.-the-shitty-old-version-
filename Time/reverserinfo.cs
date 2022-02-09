using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class reverserinfo : MonoBehaviour
{
    private int currentframe;
    bool reversed;
    bool _reversed;
    public List<List<Reversing>> reversings = new List<List<Reversing>>();
    public List<GameObject> reverseMe;
    public LayerMask noReverse;

    void Start()
    {
        GameObject[] allGOs = UnityEngine.Object.FindObjectsOfType<GameObject>();
        Debug.Log(reverseMe.Count);

        for (int i = 0; i < allGOs.Length; i++){
            if (noReverse != (noReverse | (1 << allGOs[i].layer))){
                reverseMe.Add(UnityEngine.Object.FindObjectsOfType<GameObject>()[i]);
            }
        }

        foreach (var GO in reverseMe){
            reversings.Add(new List<Reversing>());
        }
        
    }

    void Update()
    {
        _reversed = reversed;
        reversed = IsReversed.Instance.isReversed;
        if(_reversed != reversed){
        
            reversed = true;
            int i = 0;
            foreach (var go in reverseMe)
            {
                SetTransform(0, go, i);
                i++;
            }
        }
    }

    void SetTransform(int index, GameObject go, int goi){
        if (go.GetComponent<Rigidbody>() != null) go.GetComponent<Rigidbody>().isKinematic = true;
        if (index > 0 && index < reversings[goi].Count){
            Reversing reversing = reversings[goi][index];
            go.transform.position = reversing.position;
            go.transform.rotation = reversing.rotation;
            go.transform.localScale = reversing.scale;
        }
    }

    void FixedUpdate()
    {
        if(!reversed) {
            int i = 0;
            foreach (var go in reverseMe){
                reversings[i].Insert(0, new Reversing{ position = go.transform.position, rotation = go.transform.rotation, scale = go.transform.localScale});
                i++;
            }
        }
        else {
            int i = 0;
            currentframe++;
            foreach (var go in reverseMe){
                SetTransform(currentframe, go, i);
                i++;
            }
        }
    }
}
