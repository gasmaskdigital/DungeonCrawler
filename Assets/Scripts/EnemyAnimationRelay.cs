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

    public void SkeletonLightAttack()
    {
        enemyAttackHandler.SkeletonLightAttack();
    }

    public void SkeletonHeavyAttack()
    {
        enemyAttackHandler.SkeletonHeavyAttack();
    }

    public void TrollLightAttack()
    {        
        enemyAttackHandler.TrollLightAttack();
    }

    public void TrollHeavyAttack()
    {        
        enemyAttackHandler.TrollHeavyAttack();
    }

    public void SpiderLightAttack()
    {
        enemyAttackHandler.SpiderLightAttack();
    }

    public void SpiderHeavyAttack()
    {
        enemyAttackHandler.SpiderHeavyAttack();
    }

    public void OrcHeavyAttack()
    {
        enemyAttackHandler.OrcHeavyAttack();
    }

    public void OrcLightAttack()
    {
        enemyAttackHandler.OrcLightAttack();
    }

}
