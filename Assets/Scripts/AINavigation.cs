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
    private float roamRange = 20f;
    private float currentspeed;

    private Vector3 roamDestination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyStats = GetComponent<EnemyStats>();
        
        navMeshAgent.speed = enemyStats.moveSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            if (playerSpotted)
            {
                navMeshAgent.destination = playerHandler.transform.position;
                if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    AttackPlayer();
                }
            }

            Roaming();
        }
        currentspeed = navMeshAgent.velocity.magnitude;
        enemyAnimator.SetFloat("Speed", currentspeed, 0, Time.deltaTime);
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

    void AttackPlayer()
    {

    }
}
