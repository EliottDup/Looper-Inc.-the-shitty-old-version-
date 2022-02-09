using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor;
public class Gun : MonoBehaviour
{
    //Time
    public GameObject TimeKeeper;
    reverserinfo rev;

    //input
    MasterInput MasterInput;

    //Gun Stats
    public string GunName;
    public int damage, magSize, bulletsPerTap;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    private float newspread;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //Bools
    bool readytoshoot = true, reloading = false;
    public bool HeldByEnemy = false;
    public bool shooting = false;
    private PauseMenu PauseMenu;

    //Referenes
    public GameObject Camera, EnemyCam;
    public Transform AttackPoint;
    public RaycastHit RayHit;
    public LayerMask WhatIsEnemy, Player;
    
    //Bullet
    public GameObject BulletObject;
    private GameObject Bullet;
    private Vector3 BulletTarget;
    public float BulletSpeed = 10;
    public BulletList BL;

    //Graphics
    public TextMeshProUGUI AmmoLeft;
    public int SpinsPerReload;
    public Animator GunAnimator;

    void Awake(){
        MasterInput = new MasterInput();
        MasterInput.Enable();
        if (transform.parent.name == "EnemyGun"){
            this.enabled = true;
        }
        rev = TimeKeeper.GetComponent<reverserinfo>();
    }

    private void Start(){
        bulletsLeft = magSize;
        GunAnimator = GetComponent<Animator>();
        GunAnimator.enabled = true;
        newspread = spread;
    }

    private void Update(){
        MyInput();
        HeldByEnemy = Enemycheck();
        if (!HeldByEnemy)  AmmoLeft.SetText(GunName + "\n" + bulletsLeft + "/" + magSize);
    }

    private void MyInput(){
        if (!HeldByEnemy){
            if (allowButtonHold && MasterInput.Player.Shoot.ReadValue<float>() > 0 ) shooting = true;
            else shooting = MasterInput.Player.Shoot.triggered;
        }
        //reload
        if (!HeldByEnemy){
            if (MasterInput.Player.Reload.triggered && bulletsLeft < magSize && !reloading && readytoshoot || shooting && bulletsLeft == 0 && !reloading && readytoshoot) Reload();
        }
        else{
            if (shooting && bulletsLeft == 0 && !reloading && readytoshoot) Reload();
        }
        //Shoot
        if (readytoshoot && shooting && !reloading && bulletsLeft > 0 && !PauseMenu.GameIsPaused){
            bulletsShot = bulletsPerTap;
            Shoot();
            }
    }

    private void Shoot(){
        readytoshoot = false;

        //spread
        float spreadX = Random.Range(-newspread * 0.25f, newspread * 0.25f);
        float spreadY = Random.Range(-newspread * 0.25f, newspread * 0.25f);

        //Raycast
        if (!HeldByEnemy){
            Vector3 Spread = Camera.transform.forward + new Vector3(spreadX, spreadY, 0);
            
            if(Physics.Raycast(Camera.transform.position, Spread, out RayHit, range, ~Player)){
                BulletTarget = RayHit.point;
                
                Debug.DrawLine(Camera.transform.position, BulletTarget, Color.green, 10f);
            }
            else{
                BulletTarget = Camera.transform.position + Spread * 100;
                
                Debug.DrawLine(Camera.transform.position, BulletTarget, Color.red, 10f);
            }
        }
        else{
            if(Physics.Raycast(EnemyCam.transform.position, EnemyCam.transform.forward, out RayHit, range, ~WhatIsEnemy)){
                BulletTarget = RayHit.point;
                Debug.Log (BulletTarget);
                Debug.DrawLine(EnemyCam.transform.position, BulletTarget, Color.green, 10f);
            }
            else{
                BulletTarget = EnemyCam.transform.position + EnemyCam.transform.forward * 100;
                Debug.Log (BulletTarget);
                Debug.DrawLine(EnemyCam.transform.position, BulletTarget, Color.red, 10f);
            }
        }


        // if (!BL.recycleOrNot()){
        //     SpawnBullet(BulletTarget);
        //     }
        // else{
        //     RecycleBullet(BulletTarget);
        // }
        SpawnBullet(BulletTarget);
        GunAnimator.Play("Recoil");
        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0){
            if (newspread < 0.5) newspread += spread;
            Invoke("Shoot", timeBetweenShots);
        }
        else
        {
            newspread = spread;
        }
        Debug.Log(spreadX + " X");
        //Debug.Log(spreadY  + " Y\n" + newspread);
    }

    private void ResetShot(){
        GunAnimator.Play("Empty");
        readytoshoot = true;
    }

    private void Reload(){
        GunAnimator.speed = SpinsPerReload/reloadTime;
        ReloadAnim();
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void SpawnBullet(Vector3 BulletTarget){
        Bullet = Instantiate(BulletObject, AttackPoint.position, new Quaternion(0, 0, 0, 0), null) as GameObject;
        TimeKeeper.GetComponent<reverserinfo>().reverseMe.Add(Bullet);
        TimeKeeper.GetComponent<reverserinfo>().reversings.Add(new List<Reversing>());
        rev.reversings[rev.reversings.Count - 1].Insert(0, new Reversing{position = Vector3.zero, rotation = new Quaternion(0, 0, 0, 0)});
        BL.Bullets.Add(Bullet);
        ShootBullet(Bullet, BulletTarget);
    }

    private void RecycleBullet(Vector3 BulletTarget){
        Bullet = BL.Bullets[0];
        BL.Bullets.RemoveAt(0);
        Bullet.GetComponent<BoxCollider>().isTrigger = true;
        Bullet.GetComponent<Rigidbody>().useGravity = false;
        Bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Debug.Log();
        Bullet.transform.position = AttackPoint.position;
        Bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
        BL.Bullets.Add(Bullet);
        ShootBullet(Bullet, BulletTarget);
    }

    private void ShootBullet(GameObject Bullet, Vector3 BulletTarget){
        BulletMove bulletMove;
        BulletHit bulletHit;
        bulletMove = Bullet.GetComponent<BulletMove>();
        bulletHit = Bullet.GetComponent<BulletHit>();
        bulletMove.BulletTarget = BulletTarget;
        bulletMove.BulletSpeed = BulletSpeed;
        bulletHit.Damage = damage;
        bulletMove.Shoot();

    }

    private void ReloadFinished(){
        bulletsLeft = magSize;
        reloading = false;
        GunAnimator.Play("Empty");
    }

    private void ReloadAnim(){
        GunAnimator.Play("Reload");
    }

    void OnDisable(){
        GunAnimator.enabled = false;
        if (MasterInput != null) MasterInput.Disable();
        if (!HeldByEnemy) AmmoLeft.SetText("");
        HeldByEnemy = false;
    }

    bool Enemycheck(){
            if (transform.parent.name != "EnemyGun"){
            return false;
            
        }
        else{
            return true;
        }
    }
    
    void OnEnable(){
        if (transform.parent.name != "EnemyGun"){
            HeldByEnemy = false;
            MasterInput.Enable();
        }
        else{
            HeldByEnemy = true;
        }
    }
}