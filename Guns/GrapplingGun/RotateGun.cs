    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;
    public GameObject grapplingGun, GunPos;
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    void Update()
    {
        if (!grappling.IsGrappling())
        {
            desiredRotation = GunPos.transform.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);
        }
        grapplingGun.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
