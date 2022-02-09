using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject StaminaBar;
    PlayerMovement PM;
    
    [Header("Health")]
    
    [Range(0, 100)] public float Health = 100;

    [Header("Stamina")] //STAMINA
    [Range(0, 100)]
    public float Stamina = 100;
    public float pauseBeforeRefill = 1f;
    public float RefillSpeed = 50f;
    [Range(0.0f, 1f)]
    public float TimeSinceLastDrain = 0f;
    bool GroundHit = false;

    void Start(){
        PM = GetComponent<PlayerMovement>();
    }

    void Update(){
        UpdateStaminaValues();
        UpdateHealthStamina();
        
    }

    void UpdateHealthStamina(){
        if(Stamina > 100) Stamina = 100;
        if(Stamina < 0) Stamina = 0;
        if(Health > 100) Health = 100;
        if(Health < 0) Health = 0;

        HealthBar.transform.localScale = new Vector3(Health/100, 1, 1);
        StaminaBar.transform.localScale = new Vector3(Stamina/100, 1,1);
    }

    void UpdateStaminaValues(){
        if (TimeSinceLastDrain == 0) GroundHit = false;
        if (PM.grounded) GroundHit = true;
        if (!GroundHit) return;
        if (TimeSinceLastDrain != pauseBeforeRefill){
            TimeSinceLastDrain += Time.deltaTime;
            if (TimeSinceLastDrain >= pauseBeforeRefill) TimeSinceLastDrain = pauseBeforeRefill;
        }
        if (TimeSinceLastDrain == pauseBeforeRefill){
            Stamina += Time.deltaTime * RefillSpeed;
        }
    }
}
