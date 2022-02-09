using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public string HeldGun;
    public float Health = 100;
    [SerializeField] float Maxhealth = 100;
    [SerializeField] float attackRadius = 10, lookRadius = 20;
    [SerializeField] Gun Gun;
    [SerializeField] Transform target;
    [SerializeField] bool inAttackRange, inViewRange;
    NavMeshAgent agent;
    [SerializeField] GameObject Eyes, HealthBar, HealthBarOrientation;
    
    // Start is called before the first frame update
    void Start()
    {
        Gun = GetComponentInChildren<Gun>();
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        Gun.EnemyCam = Eyes;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReversed.Instance.isReversed) {
            agent.SetDestination(transform.position);
            agent.enabled = false;
            this.enabled = false;
        }

        if (Health > Maxhealth) Health = Maxhealth;
        if (Health <= 0){
            Gun.shooting = false;
            Gun.enabled = false;
            Gun.GetComponent<Rigidbody>().isKinematic = false;
            Gun.gameObject.transform.parent = null;
            Gun.transform.localScale = new Vector3(1, 1, 1);
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(0, -10, 0);
        }
        HeldGun = Gun.GunName;
        float distance = Vector3.Distance(target.position, transform.position);
        inViewRange = distance <= lookRadius;
        inAttackRange = distance <= attackRadius;
        
        if (inViewRange && !inAttackRange) followPlayer();
        if (inViewRange && inAttackRange) AttackPlayer();
        if (!inAttackRange) Gun.shooting = false;
    }

    void LateUpdate(){
        HealthBarOrientation.transform.LookAt(target);
        HealthBar.transform.localScale = new Vector3(Health/100, 1, 1);
    }

    void followPlayer(){
        agent.SetDestination(target.position);
    }

    void AttackPlayer(){
        agent.SetDestination(transform.position);
        transform.LookAt(target, Vector3.up);
        Eyes.transform.LookAt(target, Vector3.up);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x*0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z*0);
        Gun.shooting = true;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

    }
}
