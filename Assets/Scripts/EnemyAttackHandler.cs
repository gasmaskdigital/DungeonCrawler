using Unity.Mathematics;
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

    [Header("Sounds")]
    [SerializeField] AudioClip vampLightImpact;
    [SerializeField] AudioClip vampLightSwoosh;
    [SerializeField] AudioClip vampHeavyImpact;
    [SerializeField] AudioClip skeletonBowShoot;
    [SerializeField] AudioClip skeletonThrow;
    [SerializeField] AudioClip trollLightAttack;
    [SerializeField] AudioClip trollHeavyAttack;
    [SerializeField] AudioClip spiderLightAttack;
    [SerializeField] AudioClip spiderHeavyAttack;


    [Header("Ranged Prefabs")]
    [SerializeField] GameObject skeletonLightArrow;
    [SerializeField] GameObject skeletonBomb;


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
            case "Skeleton":
                if(attackType == AttackType.LightAttack)
                {
                    SkeletonLightAttack();
                }
                else
                {
                    SkeletonHeavyAttack();
                }
                break;
        }
    }

    public void AttackTypeCheck()
    {
        attackChance = UnityEngine.Random.Range(1, 6);
        if(attackChance >= 4 )
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

        SoundManager.Instance.PlaySound(vampLightSwoosh, transform);
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, lightAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach(Collider c in colliders)
        {
            Debug.Log(c.gameObject.name);
            if (c.CompareTag("Player"))
            {
                PlayerStats playerStats = c.GetComponent<PlayerStats>();
                

                int damageDealt = LightAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
                playerStats.TakeDamage(damageDealt);
                SoundManager.Instance.PlaySound(vampLightImpact, transform);
            }
        }
    }

    private void VampireHeavyAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, heavyAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach(Collider c in colliders)
        {
            PlayerStats playerStats = c.GetComponent<PlayerStats>();

            int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);

            playerStats.TakeDamage(damageDealt);
            SoundManager.Instance.PlaySound(vampHeavyImpact, transform);
        }
    }

    public void SkeletonLightAttack()
    {

        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;
        

        GameObject arrow = Instantiate(skeletonLightArrow, spawnPoint, spawnRotation);
        SkeletonArrow arrowScript = arrow.GetComponent<SkeletonArrow>();
        arrowScript.owner = gameObject;

        SoundManager.Instance.PlaySound(skeletonBowShoot, transform);
    }

    public void SkeletonLightImpact(PlayerStats playerStats)
    {
        int damageDealt = LightAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
        playerStats.TakeDamage(damageDealt);
    }

    public void SkeletonHeavyAttack()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;

        GameObject bomb = Instantiate(skeletonBomb, spawnPoint, spawnRotation);
        SkeletonBomb bombScript = bomb.GetComponent<SkeletonBomb>();
        bombScript.owner = gameObject;

        SoundManager.Instance.PlaySound(skeletonThrow, transform);

    }

    public void SkeletonBombExplosion(PlayerStats playerstats)
    {
        int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
        playerstats.TakeDamage(damageDealt);
    }

    public void TrollLightAttack()
    {
        SoundManager.Instance.PlaySound(trollLightAttack, transform);
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, lightAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                PlayerStats playerStats = c.GetComponent<PlayerStats>();


                int damageDealt = LightAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
                playerStats.TakeDamage(damageDealt);
                
            }
        }
    }

    public void TrollHeavyAttack()
    {
        SoundManager.Instance.PlaySound(trollHeavyAttack, transform);
        Vector3 origin = transform.position + Vector3.up * 1f;

        Collider[] colliders = Physics.OverlapSphere(origin, heavyAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach (Collider c in colliders)
        {
            PlayerStats playerStats = c.GetComponent<PlayerStats>();

            int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);

            playerStats.TakeDamage(damageDealt);
            
        }
    }

    public void SpiderHeavyAttack()
    {
        SoundManager.Instance.PlaySound(spiderHeavyAttack, transform);
        Vector3 origin = transform.position + Vector3.up * 0.40f + transform.forward * 1.3f;

        Collider[] colliders = Physics.OverlapSphere(origin, lightAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                PlayerStats playerStats = c.GetComponent<PlayerStats>();


                int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
                playerStats.TakeDamage(damageDealt);

            }
        }

        Debug.Log("spider heavy attack");
    }

    public void SpiderLightAttack()
    {
        SoundManager.Instance.PlaySound(spiderLightAttack, transform);
        Vector3 origin = transform.position + Vector3.up * 0.40f + transform.forward * 1.1f;

        Collider[] colliders = Physics.OverlapSphere(origin, lightAttackRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                PlayerStats playerStats = c.GetComponent<PlayerStats>();


                int damageDealt = LightAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
                playerStats.TakeDamage(damageDealt);

            }
        }

        Debug.Log("spider light attack");
    }


    private int LightAttackDamage(int enemyAttack, int playerDefence, int playerEndurance)
    {
        int enemyValues = enemyAttack * 100;
        int playerValues = 100 + playerDefence + playerEndurance;
        int preDamageValue = enemyValues / playerValues;
        int damageDealt = damageDealt = Mathf.CeilToInt(preDamageValue * 1.2f);
        return damageDealt;
                
    }

    private int HeavyAttackDamage(int enemyAttack, int playerDefence, int playerEndurance)
    {
        int enemyValues = enemyAttack * 100;
        int playerValues = 100 + playerDefence + playerEndurance;
        int preDamageValue = enemyValues / playerValues;
        int damageDealt = damageDealt = Mathf.CeilToInt(preDamageValue * 1.5f);
        return damageDealt;
        
    }
   
}
