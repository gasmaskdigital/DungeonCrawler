using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour, IKnockbackable
{
    private NavMeshAgent navMeshAgent;
    private Animator enemyAnimator;
    private EnemyStats enemyStats;
    [SerializeField] PlayerHandler playerHandler;    
    private bool playerInAttackRange = false;
    public bool playerSpotted = false;
    private float roamRange = 40f;
    private float currentspeed;
    public bool canMove = true;
    public string enemyName;
    public LayerMask playerMask;
    public bool isAttacking = false;
    private float knockDelay = 0.5f;
    private EnemyAttackHandler enemyAttackHandler;
    public static bool playerAlive = true;
    public bool alive = true;
    private CapsuleCollider capsuleCollider;


    private Rigidbody rb;

    private Vector3 roamDestination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody>();
        enemyAttackHandler = GetComponent<EnemyAttackHandler>();
        capsuleCollider = GetComponent<CapsuleCollider>();


        GameObject player = GameObject.FindWithTag("Player");
        playerHandler = player.GetComponent<PlayerHandler>();
        
        navMeshAgent.speed = enemyStats.moveSpeed;
        enemyName = enemyStats.enemyName;

        Roaming();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                currentspeed = navMeshAgent.velocity.magnitude;
                enemyAnimator.SetFloat("Speed", currentspeed, 0, Time.deltaTime);



                if (canMove)
                {

                    if (playerSpotted && playerAlive)
                    {
                        navMeshAgent.destination = playerHandler.transform.position;
                        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && !isAttacking)
                        {
                            AttackPlayer();
                        }
                    }
                    else if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                    {
                        Roaming();
                    }

                }
            }

            
        }
    }

   void Roaming()
    {
        roamDestination = transform.position + Random.insideUnitSphere * roamRange;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(roamDestination, out hit, 1.0f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(roamDestination);
        }
    }

    public void ChasePlayer()
    {
        playerSpotted = true;
        
    }

  
    void AttackPlayer()
    {
        if (!alive)
            return;
        enemyAttackHandler.AttackTypeCheck();
        isAttacking = true;
    }

    public void CheckEnemyNameForAttack()
    {
        // checking what enemy name to apply the correct attack logic. Called from animation relay

        switch(enemyName)
        {
            case "Vampire":
                VampireAttack();
                break;


        }
    }

    void VampireAttack()
    {
        

        Vector3 origin = transform.position + Vector3.up * 1.5f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, enemyStats.enemyAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach(Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Player"))
            {
                Destroy(c.gameObject);
                Roaming();
            }
        }
    }

    public void GetKnockBack(float force, Vector3 playerpos)
    {
        canMove = false;
        Vector3 knockbackDirection = (transform.position - playerpos).normalized;
        StartCoroutine(ApplyKnockBack(force, knockbackDirection));
    }

    private IEnumerator ApplyKnockBack(float force, Vector3 knockbackDirection)
    {
        yield return null;
        navMeshAgent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;


        rb.AddForce(knockbackDirection * force, ForceMode.Impulse);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.05f);
        yield return new WaitForSeconds(1.2f);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        navMeshAgent.Warp(transform.position);
        navMeshAgent.enabled = true;

        yield return null;

        canMove = true;
    }

   public void CanMoveOff()
    {
        canMove = false;

        if (navMeshAgent.gameObject.activeInHierarchy && navMeshAgent.isOnNavMesh)
        {
            if (isAttacking)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;
            }
        }
        
    }

    public void CanMoveOn()
    {
        if (alive)
        {
            canMove = true;
            navMeshAgent.isStopped = false;
            isAttacking = false;
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DisableNavAndCollider()
    {

        if (navMeshAgent.enabled)
        {

            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }
        

    }

    public void ResetTriggeredBool()
    {
        playerSpotted = false;
    }
   
}

public interface IKnockbackable
{
    void GetKnockBack(float force, Vector3 playerPos);
}