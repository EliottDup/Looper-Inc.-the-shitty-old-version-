using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 GrapplePoint, HookPos;
    private Quaternion HookRot;
    public LayerMask WhatIsGrappelable;
    public Transform GunTip, Camera, player, HookDefPos;
    private float maxDistance = 100f, staminaDrain = 5f, test;
    private SpringJoint Joint;
    public GameObject Hook;
    public float Hookspeed = 10f;
    private PauseMenu PauseMenu;
    [SerializeField] PlayerStats PS;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        //PS = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        if (Input.GetButtonDown("Fire1") && PS.Stamina > 0)
        {
            StartGrapple();
        }
        else if (Input.GetButtonUp("Fire1") || PS.Stamina <= 0)
        {
            StopGrapple();
        }
        
        if (IsGrappling()){
            PS.Stamina -= (staminaDrain * Time.deltaTime);
            PS.TimeSinceLastDrain = 0;
        }
    }

    void LateUpdate()
    {
        drawRope();
        moveHook();
    }

    void OnDisable(){
        StopGrapple();
        Hook.transform.position = HookDefPos.transform.position;
        Hook.transform.rotation = HookRot;
    }

    void StartGrapple()
    {
        if(PauseMenu.GameIsPaused) return;
        RaycastHit hit;
            if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance, WhatIsGrappelable)){
            GrapplePoint = hit.point;
            Joint = player.gameObject.AddComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
            Joint.connectedAnchor = GrapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, GrapplePoint);


            Joint.maxDistance = distanceFromPoint * .8f;
            Joint.minDistance = distanceFromPoint * .25f;

            Joint.spring = 4.5f;
            Joint.damper = 3.5f;
            Joint.massScale = 4.5f;

            lr.positionCount = 2;
            HookRot = Camera.transform.rotation;
            HookPos = GrapplePoint;
        }
    }


    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(Joint);
    }

    void drawRope()
    {
        if (!Joint) return;
        lr.SetPosition(0, GunTip.position);
        lr.SetPosition(1, GrapplePoint);
    }

    public bool IsGrappling()
    {
        return Joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
            return GrapplePoint;
    }

    void moveHook()
    {
        if (!IsGrappling()){
            HookPos = HookDefPos.position;
            HookRot = HookDefPos.rotation;
        }
        Hook.transform.position = Vector3.Lerp(Hook.transform.position, HookPos, Time.deltaTime * Hookspeed);
        Hook.transform.rotation = HookRot;
        
    }

    public int getLayernum(LayerMask layer){
        return (int) Mathf.Log(Mathf.Abs(Convert.ToInt64(layer.value)), 2);
    }     
}
