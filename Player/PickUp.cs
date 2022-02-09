using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject GunPos, HeldItem, Camera;
    public bool isHoldingItem = false;
    public float maxDistance = 5f;
    public LayerMask Guns;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReversed.Instance.isReversed){
            if (isHoldingItem) dropGun();
            this.enabled = false;
        }
        if(Input.GetKeyDown("e") && !isHoldingItem){
            grabGun();
            Debug.Log("e pressed");
        }
        else if(Input.GetKeyDown("e") && isHoldingItem){
            dropGun();
        }
    }

    private void grabGun(){
        RaycastHit hit;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, maxDistance, Guns)){
                Debug.Log("gun E'd");
                HeldItem = hit.transform.gameObject;
                if(HeldItem.GetComponent<Gun>() != null) if (HeldItem.GetComponent<Gun>().HeldByEnemy) return;
                isHoldingItem = true;
                HeldItem.transform.parent = GunPos.transform;
                HeldItem.transform.position = GunPos.transform.position;
                HeldItem.transform.rotation = GunPos.transform.rotation;
                HeldItem.transform.localScale = GunPos.transform.localScale;
                HeldItem.GetComponent<Rigidbody>().useGravity = false;
                HeldItem.GetComponent<Rigidbody>().isKinematic = true;
                HeldItem.GetComponentInChildren<BoxCollider>().enabled = false;
                enableScripts(true, HeldItem.gameObject);
                if (HeldItem.GetComponent<Animator>() != null){
                    HeldItem.GetComponent<Animator>().enabled = true;
                }
            }
    }

    public void dropGun(){
        if (!isHoldingItem) return;
        isHoldingItem = false;
        HeldItem.transform.parent = null;
        HeldItem.GetComponent<Rigidbody>().isKinematic = false;
        HeldItem.GetComponent<Rigidbody>().useGravity = true;
        HeldItem.GetComponentInChildren<BoxCollider>().enabled = true;
        enableScripts(false, HeldItem.gameObject);
        if (HeldItem.GetComponent<Animator>() != null){
            HeldItem.GetComponent<Animator>().enabled = false;
        }
        HeldItem = null;
    }

    private void enableScripts(bool enable, GameObject objectwithscripts){
        MonoBehaviour[] scripts = objectwithscripts.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour script in scripts){
            script.enabled = enable;
        }
    }
}