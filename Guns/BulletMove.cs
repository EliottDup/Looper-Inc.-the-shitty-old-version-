using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public Vector3 BulletTarget;
    public float BulletSpeed;
    public Vector3 BulletStartPos;


    public void Shoot(){
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        transform.LookAt(BulletTarget);
        this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.forward* BulletSpeed);
    }

    void Update()
    {
        if (IsReversed.Instance.isReversed){
            this.gameObject.GetComponent<BoxCollider>().isTrigger = true;   
        }    
    }
}
