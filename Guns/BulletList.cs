using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletList : MonoBehaviour
{
    [SerializeField] int maxBullets = 100;
    public List<GameObject> Bullets;
    public bool recycleOrNot(){
        if(Bullets.Count >= maxBullets) return true;
        return false;
    }
}
