using UnityEngine;

public class EnemyAnimationRelay : MonoBehaviour
{
    private AINavigation aiNavigation;
    private EnemyAttackHandler enemyAttackHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiNavigation = GetComponentInParent<AINavigation>();
        enemyAttackHandler = GetComponentInParent<EnemyAttackHandler>();

    }

    public void CheckEnemyAttack()
    {
        enemyAttackHandler.CheckEnemyAttack();
    }

    public void CanMoveToggle()
    {
        aiNavigation.CanMoveToggle();
    }
    
}
