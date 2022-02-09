using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public LayerMask Player, PlayerRev;
    public LayerMask Enemy;
    public float Damage;
    PlayerStats PS, PSR;

    void Start(){
        PS = PlayerManager.instance.player.GetComponent<PlayerStats>();
        PSR = PlayerManager.instance.playerRev.GetComponent<PlayerStats>();
    }
    void OnTriggerEnter(Collider other){
        if (Player == (Player | (1 << other.gameObject.layer))){
            Debug.Log(Damage);
            PS.Health -= Damage;
        }
        else if(Enemy == (Enemy | 1 << other.gameObject.layer)){
            other.GetComponent<EnemyController>().Health -= Damage;
        }
        if (PlayerRev == (PlayerRev | (1 << other.gameObject.layer))){
            Debug.Log(Damage);
            PSR.Health -= Damage;
        }
    }
}
