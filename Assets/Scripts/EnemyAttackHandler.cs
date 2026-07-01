using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour

{
    [Header("Refs")]
    public LayerMask playerMask;
    private AINavigation AINavigation;
    private EnemyStats enemystats;
    public AttackType enemyAttackType;
    private int attackChance;
    private AttackType attackType;
    public float lightAttackRadius;
    public float heavyAttackRadius;
    private Animator enemyAnimator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AINavigation = GetComponent<AINavigation>();
        enemystats = GetComponent<EnemyStats>();
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    

    public void CheckEnemyAttack()
    {
        switch (AINavigation.enemyName)
        {
            case "Vampire":
                if(attackType == AttackType.LightAttack)
                {
                    VampireLightAttack();
                }
                else
                {
                    VampireHeavyAttack();
                }
                break;
        }
    }

    public void AttackTypeCheck()
    {
        attackChance = Random.Range(1, 5);
        if(attackChance == 5)
        {
            attackType = AttackType.HeavyAttack;
            enemyAnimator.SetTrigger("HeavyAttack");
        }
        else
        {
            attackType = AttackType.LightAttack;
            enemyAnimator.SetTrigger("LightAttack");
        }
    }

    private void VampireLightAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, lightAttackRadius, playerMask);
        foreach(Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                PlayerStats playerStats = c.GetComponent<PlayerStats>();

                int damageDealt = LightAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
                playerStats.TakeDamage(damageDealt);
            }
        }
    }

    private void VampireHeavyAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, heavyAttackRadius, playerMask);
        foreach(Collider c in colliders)
        {
            PlayerStats playerStats = c.GetComponent<PlayerStats>();

            int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);

            playerStats.TakeDamage(damageDealt);
        }
    }

    private int LightAttackDamage(int enemyAttack, int playerDefence, int playerEndurance)
    {
        enemyAttack = enemyAttack * enemyAttack;
        int playervalues = playerDefence + playerEndurance;
        playervalues = playervalues * playervalues;
        float damageFloat;
        damageFloat = (enemyAttack / playervalues) * 1.2f;
       int damageDealt = (int)damageFloat;
            return damageDealt;
    }

    private int HeavyAttackDamage(int enemyAttack, int PlayerDefence, int PlayerEndurance)
    {
        enemyAttack = enemyAttack * enemyAttack;
        int playerValues = PlayerDefence + PlayerEndurance;
        playerValues = playerValues * playerValues;
        float damageFloat = (enemyAttack / playerValues) * 1.5f;
        int damageDealt = (int)damageFloat;

        Time.timeScale = 0f;

        return damageDealt;
    }
   
}
