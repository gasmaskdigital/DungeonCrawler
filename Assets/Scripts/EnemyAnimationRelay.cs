using UnityEngine;

public class EnemyAnimationRelay : MonoBehaviour
{
    private AINavigation aiNavigation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiNavigation = GetComponentInParent<AINavigation>();
    }

    public void HeavyAttack()
    {
        aiNavigation.CheckEnemyNameForAttack();
    }

    public void CanMoveToggle()
    {
        aiNavigation.CanMoveToggle();
    }
    
}
