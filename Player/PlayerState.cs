using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    [SerializeField] Rigidbody RB;
    [SerializeField] PlayerMovement PM;
    [SerializeField] Wallrunning WR;
    private MasterInput MasterInput;
    public enum State_Movement{
        State_Standing,
        State_Running,
        State_Jumping,
        State_WallRunning,
        State_Sliding,
        State_Crouching
    };

    public enum State_Gun{
        State_NoGun,
        State_HoldingGun,
        State_Aiming,
        State_Shooting
    };

    public PlayerState.State_Movement Move_state;
    public PlayerState.State_Gun Gun_state;

    void Awake(){
        WR = GetComponent<Wallrunning>();
        RB = GetComponent<Rigidbody>();
        PM = GetComponent<PlayerMovement>();
        MasterInput = new MasterInput();
        Move_state = State_Movement.State_Standing;
    }

    void Update(){
        if (MasterInput.Player.Movement.ReadValue<Vector2>().x == 0 && MasterInput.Player.Movement.ReadValue<Vector2>().y == 0 ) Move_state = State_Movement.State_Standing;
        else if(MasterInput.Player.Movement.ReadValue<Vector2>().x > 0 || MasterInput.Player.Movement.ReadValue<Vector2>().y > 0 ) Move_state = State_Movement.State_Running;
        else if(PM.crouching && RB.velocity == Vector3.zero) Move_state = State_Movement.State_Crouching;
        else if(PM.crouching && RB.velocity != Vector3.zero) Move_state = State_Movement.State_Sliding;
        else if(PM.jumping) Move_state = State_Movement.State_Jumping;
        else if(WR.isWallrunning()) Move_state = State_Movement.State_WallRunning;
    }
}
