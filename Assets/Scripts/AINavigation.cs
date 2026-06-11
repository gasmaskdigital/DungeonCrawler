using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator enemyAnimator;
    private EnemyStats enemyStats;

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
        
    }
}
