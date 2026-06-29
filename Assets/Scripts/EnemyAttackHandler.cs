using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour

{
    [Header("Refs")]
    private AINavigation AINavigation;
    private EnemyStats enemystats;
    public AttackType enemyAttackType;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AINavigation = GetComponent<AINavigation>();
        enemystats = GetComponent<EnemyStats>();        
    }

    public void CheckEnemyAttack()
    {
        switch (AINavigation.enemyName) 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
