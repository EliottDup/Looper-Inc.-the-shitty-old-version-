using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private LineRenderer lr;
    private SpringJoint Joint;
    private GameObject GrabObject;
    public LayerMask Grabbable;
    public Transform Camera, GrabPoint;
    private float maxDistance = 5f;

    

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update(){
        if (Input.GetMouseButtonDown(1)) {
            Grab();
        }
        else if (Input.GetMouseButtonUp(1)) {
            LetGo();
        }
        if (!Joint) return;
        Joint.connectedAnchor = GrabPoint.position;
    }

    void LateUpdate(){
        drawLine();
    }

    void Grab() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance, Grabbable)){
            Debug.Log("Works");
            GrabObject = hit.transform.gameObject;
            Joint = GrabObject.AddComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
            Joint.connectedAnchor = GrabPoint.position;

            float distanceFromObject = Vector3.Distance(GrabPoint.transform.position, GrabObject.transform.position);


            Joint.maxDistance = distanceFromObject * 0.001f;
            Joint.minDistance = distanceFromObject * 0f;

            Joint.spring = 7f;
            Joint.damper = 1f;
            Joint.massScale = 10000f;

            lr.positionCount = 2;
            
            GrabObject.layer = 7;
        }
    }

    void LetGo(){
        lr.positionCount = 0;
        Destroy(Joint);
    }

    void drawLine(){
        if (!Joint) return;
        lr.SetPosition(0, GrabPoint.position);
        lr.SetPosition(1, GrabObject.transform.position);
        
    }

    public int getLayernum(LayerMask layer){
        return (int) Mathf.Log(Mathf.Abs(Convert.ToInt64(layer.value)), 2);
    }
}
