using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{   
    [Header("GameObjects")]
    public GameObject Cam;
    public GameObject Orientation;
    Collider coll;
    [Header("detection")]
    [SerializeField] LayerMask notWallrunabble;
    PlayerStats PS;
    [SerializeField] float wallAttachUpForce = 10;
    [SerializeField] float WallMaxDistance = 1f;
    [SerializeField] float wallSpeedMultiplier = 1.2f;
    [SerializeField] float minimumHeight = 1.2f;
    [SerializeField] float maxAngleRoll = 20;
    [Range(0.0f, 1.0f)]
    [SerializeField] float normalizedAngleThreshold;
    [SerializeField] float WallRunTime = 2;
    [SerializeField] float JumpUpForce = 2.5f;
    [SerializeField] float jumpDuration = 1f;
    [SerializeField] float wallBouncing = 3f;
    [SerializeField] float cameraTransitionDuration = 1;
    [SerializeField] float wallGravityforce = 20f;

    public PlayerMovement PM;
    Vector3[] directions;
    RaycastHit[] hits;


    Vector3 alongthewall;
    Vector3 alongthewall2;
    bool wallRunning = false;
    Vector3 lastWallpos;
    Vector3 lastWallNorm;
    float elapsedTimeSinceJump = 0;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetatch = 0;
    bool jumping;
    float[] dist;
    bool wallRunCompare = false;
    Rigidbody rb;
    float t = 0;

    bool IsGrounded() => PM.grounded;
    public bool isWallrunning() => wallRunning;

    bool CanWallRun(){
        float verticalAxis = Input.GetAxisRaw("Vertical");

        return !IsGrounded() && verticalAxis > 0 && verticalCheck();
    }

    bool verticalCheck(){
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight);
    }

    void Start(){
        PS = GetComponent<PlayerStats>();
        coll = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        PM = GetComponent<PlayerMovement>();
        directions = new Vector3[]{
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.left
        };
    }

    void Update(){
        if (wallRunning){
            PS.Stamina -= (Time.deltaTime / WallRunTime) * 100;
            if (PS.Stamina <= 0) Jump();
        }
    }

    void LateUpdate(){
        if (wallRunning != wallRunCompare) rb.AddForce(Vector3.up * wallAttachUpForce);
        Debug.DrawRay(transform.position, Orientation.transform.TransformDirection(transform.forward), Color.blue);
        wallRunning = false;

        if (CanAttatch()){
            hits = new RaycastHit[directions.Length];

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = Orientation.transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, dir, out hits[i], WallMaxDistance, ~notWallrunabble);
                if (hits[i].collider != null){
                    Debug.DrawRay(transform.position, dir * hits[i].distance * 10, Color.cyan);
                }
                else{
                    Debug.DrawRay(transform.position, dir * hits[i].distance * 10, Color.red);
                }
            }

            if(CanWallRun()){
                int tmp = -1;                               //find closest hit
                for (int i = 0; i < hits.Length; i++){ 
                    if (hits[i].distance < tmp || tmp < 0){
                        if (hits[i].collider != null){
                            tmp = i;
                        }
                    }
                }
                if (hits.Length > 0 && tmp != -1){
                    OnWall(hits[tmp]);
                    lastWallpos = hits[tmp].point;
                    lastWallNorm = hits[tmp].normal;
                }
            }
        }
        if (wallRunning){
            rb.useGravity = false;
            elapsedTimeSinceWallDetatch = 0;
            elapsedTimeSinceWallAttach += Time.deltaTime;
            ApplyForces();
            Debug.DrawRay(transform.position, getWallJumpDir() * 5, Color.yellow);
            PS.TimeSinceLastDrain = 0f;
        }
        else{
            rb.useGravity = true;
            elapsedTimeSinceWallAttach = 0;
            elapsedTimeSinceWallDetatch += Time.deltaTime;

        }
        Vector3 CamEuler = Cam.transform.rotation.eulerAngles;
        CamEuler.z = GetCameraRoll();
        Cam.transform.rotation = Quaternion.Euler(CamEuler);
        myInput();
    }

    void myInput(){
        if (!wallRunning) return;
        if (Input.GetButton("Jump")){
            Jump();
            }
    }

    void ApplyForces(){
        rb.AddForce(Vector3.down * wallGravityforce * 75 * Time.deltaTime); //Gravity
        //rb.AddForce();
    }
    bool CanAttatch(){
        if(jumping){
            elapsedTimeSinceJump += Time.deltaTime;
            if (elapsedTimeSinceJump < jumpDuration){
                elapsedTimeSinceJump = 0;
                jumping = false;
            }
            return false;
        }
        else if(PS.Stamina <= 0) return false;
        return true;
    }

    void OnWall(RaycastHit hit){
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if (d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold){
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 alongWall = Orientation.transform.TransformVector(transform.forward);
            
            //Debug.DrawRay(transform.position, alongWall * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNorm * 10, Color.magenta);
            Vector3 targetVelocity = Direction() * vertical * wallSpeedMultiplier * (PM.moveSpeed/175);
            if (rb.velocity != Direction() * vertical * wallSpeedMultiplier * (PM.moveSpeed/175)){
                t += Time.deltaTime / 2f;
                if (t > 1f) t = 1f;
                rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, t);
                rb.AddForce(Vector3.up * wallAttachUpForce);
            }
            if (rb.velocity == Direction() * vertical * wallSpeedMultiplier * (PM.moveSpeed/175)){
                t = 0;
            }
            wallRunning = true;
        }
    }

    float CalcSide(){
        if (wallRunning){
            Vector3 heading = lastWallpos - transform.position;
            Vector3 perp = Vector3.Cross(Orientation.transform.forward, heading);
            float dir = Vector3.Dot(perp, transform.up);
            return dir;
        }
        return 0;
    }

    Vector3 Direction(){
        alongthewall = Vector3.Cross(Vector3.up, lastWallNorm);
        float wallAngle = Vector3.Angle(Orientation.transform.forward, alongthewall);
        alongthewall2 = Vector3.Cross(lastWallNorm, Vector3.up);
        float wallAngle2 = Vector3.Angle(Orientation.transform.forward, alongthewall2);
        float angle = Mathf.Min(wallAngle, wallAngle2);
        
        Debug.DrawRay(transform.position, alongthewall * 10, Color.black);
        Debug.DrawRay(transform.position, alongthewall2 * 10, Color.black);
        if (angle == wallAngle){
            return alongthewall;
        }
        else{
            return alongthewall2;
        }
    }

    float GetCameraRoll(){
        float dir = CalcSide();
        float cameraAngle = Cam.transform.eulerAngles.z;
        float targetAngle = 0;
        if (dir != 0){
            if (Direction() == alongthewall){
                targetAngle = Mathf.Sign(dir) * maxAngleRoll;
            }
            else{
                targetAngle = Mathf.Sign(dir) * -maxAngleRoll;
            }
        } 
        return Mathf.Lerp(cameraAngle, targetAngle, Mathf.Max(elapsedTimeSinceWallAttach, elapsedTimeSinceWallDetatch) / cameraTransitionDuration);
    }

    public Vector3 getWallJumpDir(){
        if(wallRunning){
            return (lastWallNorm * wallBouncing) + (Vector3.up * JumpUpForce);
        }
        return Vector3.zero;
    }

    public void Jump(){
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity += (getWallJumpDir() * PM.jumpForce * 0.01f);
        elapsedTimeSinceJump = 0;
        jumping = true;
        wallRunning = false;
    }
}
