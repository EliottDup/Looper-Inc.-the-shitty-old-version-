using System.ComponentModel;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseRoomTrigger : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    bool triggered, doorTime;
    public GameObject cam, Door;
    public Transform MainCam;
    GameObject Player, PlayerRev;
    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerManager.instance.player;
        PlayerRev = PlayerManager.instance.playerRev;   
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player && !IsReversed.Instance.isReversed){
            Invoke ("Doors", 1);
        }
    }

    void Doors(){
        //Door.transform.position = Door.transform.position + new Vector3(0, 4.25f, 0);
        triggered = true;
        Player.GetComponent<PlayerMovement>().enabled = false;
        Invoke("ReversePlayer", 2);
    }

    void ReversePlayer(){
        PlayerRev.GetComponent<Rigidbody>().isKinematic = false;
        triggered = false;
        Invoke("Doortime", 1);
        IsReversed.Instance.isReversed = true;
        PlayerRev.transform.position = Player.transform.position + offset;
        float tmp = cam.transform.rotation.x;
        MonoBehaviour[] scripts = Player.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour script in scripts){
            script.enabled = false;
        }
        scripts = PlayerRev.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour script in scripts){
            script.enabled = true;
        }

        // PlayerRev.GetComponent<PlayerMovement>().Xtra = tmp;
        // MainCam.transform.localRotation = Quaternion.Euler(tmp, 0, 0);
    }

    void Doortime(){
        doorTime = true;
    }

    Vector3 doortarget{get{return Door.transform.position + new Vector3(0, 4.25f, 0);}}
    Vector3 doortarget2{get{return Door.transform.position - new Vector3(0, 4.25f, 0);}}

    void Update()
    {
        if (triggered){
            var targetRotation = Quaternion.LookRotation(Door.transform.Find("Door").transform.position - cam.transform.position);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, 2f * Time.deltaTime);
            Door.transform.position = Vector3.Lerp(Door.transform.position, doortarget, Time.deltaTime/2);
        }
        if (doorTime){
            Door.transform.position = Vector3.Lerp(Door.transform.position, doortarget, Time.deltaTime/2);
        }
    }

}
