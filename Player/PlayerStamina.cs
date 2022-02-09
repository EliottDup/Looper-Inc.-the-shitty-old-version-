using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] GameObject StaminaBar;
    [Range(0.0f, 100.0f)]
    public float Stamina = 100;
    
    public float pauseBeforeRefill = 1f;
    public float RefillSpeed = 50f;
    [Range(0.0f, 1f)]
    public float TimeSinceLastDrain = 0f;

    void Start(){
        Stamina = 100;
    }

    bool tmpGrounded;

    bool Canrefill(){
        if (TimeSinceLastDrain < pauseBeforeRefill){
            return false;
        }
        return true;
    }

    void Update(){
        if (TimeSinceLastDrain != pauseBeforeRefill){
            TimeSinceLastDrain += Time.deltaTime;
            if (TimeSinceLastDrain >= pauseBeforeRefill) TimeSinceLastDrain = pauseBeforeRefill;
        }
        if (Canrefill()){
            Stamina += Time.deltaTime * RefillSpeed;
        }
        if (Stamina < 0) Stamina = 0;
        if (Stamina > 100) Stamina = 100;
        StaminaBar.transform.localScale = new Vector3(Stamina/100, 1, 1);
    }
}
