using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
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
    private string enemyName;
    public LayerMask playerMask;
    private bool isAttacking = false;

    private Vector3 roamDestination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyStats = GetComponent<EnemyStats>();

        GameObject player = GameObject.FindWithTag("Player");
        playerHandler = player.GetComponent<PlayerHandler>();
        
        navMeshAgent.speed = enemyStats.moveSpeed;
        enemyName = enemyStats.enemyName;

        Roaming();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            currentspeed = navMeshAgent.velocity.magnitude;
            enemyAnimator.SetFloat("Speed", currentspeed, 0, Time.deltaTime);



            if (canMove)
            {

                if (playerSpotted)
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
        Debug.Log("Chasing Player");
    }

    public void CanMoveToggle()
    {
        Debug.Log("Enemy Can Move Toggle");

        if (canMove)
        {
            
            canMove = false;
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
        else
        {
            if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                canMove = true;
                navMeshAgent.isStopped = false;
                isAttacking = false;
            }
        }
    }


    void AttackPlayer()
    {
        enemyAnimator.SetTrigger("Heavy Attack");
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
        Debug.Log("Vampire Attack");

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * 1.5f + transform.forward * 0.75f;
        Gizmos.DrawWireSphere(origin, enemyStats != null ? enemyStats.enemyAttackRadius : 0.5f);
    }
}
