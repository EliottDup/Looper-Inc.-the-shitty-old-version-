using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeRotator : MonoBehaviour
{
    public Transform orientation;

    void Update(){
        this.gameObject.transform.rotation = orientation.rotation;
    }
}
