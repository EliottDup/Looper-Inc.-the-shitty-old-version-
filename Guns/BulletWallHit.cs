using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWallHit : MonoBehaviour
{
    Rigidbody RB;
    float speed, frameSpeed, distance;
    [SerializeField] LayerMask bulletGoThrough;
    RaycastHit Hit;
    void Awake(){
        RB = GetComponent<Rigidbody>();
    }
    void Update(){
        speed = RB.velocity.magnitude;
        frameSpeed = Time.deltaTime;
        distance = speed * frameSpeed * 2;
        Vector3 newloc = transform.position + transform.forward * distance;
        Debug.DrawLine(transform.position, newloc, Color.cyan);
        if (Physics.Raycast(transform.position, transform.forward, out Hit, distance)){
            if (bulletGoThrough != (bulletGoThrough | (1 << Hit.transform.gameObject.layer))){
            RB.velocity = Vector3.zero;   
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            }
            RB.useGravity = true;
            transform.position = Hit.point;
        }
    }
}
