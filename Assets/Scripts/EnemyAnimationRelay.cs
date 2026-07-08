using UnityEngine;

public class EnemyAnimationRelay : MonoBehaviour
{
    private AINavigation aiNavigation;
    private EnemyAttackHandler enemyAttackHandler;
    private EnemyStats enemyStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiNavigation = GetComponentInParent<AINavigation>();
        enemyAttackHandler = GetComponentInParent<EnemyAttackHandler>();
        enemyStats = GetComponentInParent<EnemyStats>();

    }

    public void CheckEnemyAttack()
    {
        enemyAttackHandler.CheckEnemyAttack();
    }

    

    public void CanMoveOff()
    {

        aiNavigation.CanMoveOff();
        
    }

    public void CanMoveOn()
    {
        aiNavigation.CanMoveOn();
        
    }

    

    public void Death()
    {
        enemyStats.EnemyDeath();
    }
    
}
