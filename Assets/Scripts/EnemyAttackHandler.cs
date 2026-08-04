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
    private VFXManager vfxManager;

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
    [SerializeField] AudioClip orcLightSpawn;
    [SerializeField] AudioClip orcHeavyAttack;
    [SerializeField] AudioClip orcLightImpact;
    [SerializeField] AudioClip tortLightAttack;
    [SerializeField] AudioClip tortHeavyAttack;
    [SerializeField] AudioClip tortHeavyImpact;

    [Header("Ranged Prefabs")]
    [SerializeField] GameObject skeletonLightArrow;
    [SerializeField] GameObject skeletonBomb;
    [SerializeField] GameObject orcLightMagic;
    [SerializeField] GameObject spiderSummon;
    [SerializeField] GameObject skeletonSummon;
    [SerializeField] GameObject orcSummonVfx;
    [SerializeField] GameObject tortHeavyObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AINavigation = GetComponent<AINavigation>();
        enemystats = GetComponent<EnemyStats>();
        enemyAnimator = GetComponentInChildren<Animator>();
        vfxManager = FindAnyObjectByType<VFXManager>();
    }

    /*
    private void Update() // Jamie Remove This
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            enemyAnimator.SetTrigger("LightAttack");
        }
    }
    */

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

        SoundManager.Instance.PlaySound(vampLightSwoosh, transform, 0.85f);
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
                SoundManager.Instance.PlaySound(vampLightImpact, transform, 0.7f);
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
            SoundManager.Instance.PlaySound(vampHeavyImpact, transform, 1.25f);
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

        SoundManager.Instance.PlaySound(skeletonBowShoot, transform.transform, 0.9f);
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

        SoundManager.Instance.PlaySound(skeletonThrow, transform, 0.85f);

    }

    public void SkeletonBombExplosion(PlayerStats playerstats)
    {
        int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
        playerstats.TakeDamage(damageDealt);
    }

    public void TrollLightAttack()
    {
        SoundManager.Instance.PlaySound(trollLightAttack, transform, 0.85f);
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
        SoundManager.Instance.PlaySound(trollHeavyAttack, transform, 1f);
        Vector3 origin = transform.position + Vector3.up * 1f;

        vfxManager.TrollHeavyEffect(transform.position);

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
        SoundManager.Instance.PlaySound(spiderHeavyAttack, transform, 0.65f);
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
        SoundManager.Instance.PlaySound(spiderLightAttack, transform, 0.8f);
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
    }

    public void OrcLightAttack()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.75f;

        quaternion spawnRotation = transform.rotation;


        GameObject orcMagic = Instantiate(orcLightMagic, spawnPoint, spawnRotation);
        OrcLightScript magicScript = orcMagic.GetComponent<OrcLightScript>();
        magicScript.owner = gameObject;

        SoundManager.Instance.PlaySound(orcLightSpawn, transform, 1.15f);
    }

    public void OrcLightImpact(PlayerStats playerStats)
    {
        int damageDealt = LightAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
        playerStats.TakeDamage(damageDealt);
        SoundManager.Instance.PlaySound(orcLightImpact, transform, 1.1f);
    }

    public void OrcHeavyAttack()
    {
        Vector3 spawnPoint = transform.position + transform.forward * 3.5f;

        quaternion spawnRotation = transform.rotation;

        int summonIndex = UnityEngine.Random.Range(1, 6);

        if(summonIndex <= 30)
        {
            Instantiate(spiderSummon, spawnPoint, spawnRotation);
            GameObject summonVFX = Instantiate(orcSummonVfx, spawnPoint, spawnRotation);
            Destroy(summonVFX, 1.5f);
        }
        else
        {
            Instantiate(skeletonSummon, spawnPoint, spawnRotation);
            GameObject summonVFX = Instantiate(orcSummonVfx, spawnPoint, spawnRotation);
            Destroy(summonVFX, 1.5f);
        }
        SoundManager.Instance.PlaySound(orcHeavyAttack, transform, 0.85f);

    }

    public void TortLightAttack()
    {

        SoundManager.Instance.PlaySound(tortLightAttack, transform, 0.8f);
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
    }

    public void TortHeavyAttack()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y +=3.5f;

        quaternion spawnRotation = transform.rotation;


        GameObject tortHeavy = Instantiate(tortHeavyObject, spawnPoint, spawnRotation);
        TortHeavyScript tortHeavyScript = tortHeavy.GetComponent<TortHeavyScript>();
        tortHeavyScript.owner = gameObject;

        SoundManager.Instance.PlaySound(tortHeavyAttack, transform, 1.1f);
    }

    public void TortHeavyImpact(PlayerStats playerStats)
    {
        int damageDealt = HeavyAttackDamage(enemystats.attack, PlayerStats.currentDefenceTotal, PlayerStats.enduranceStat);
        playerStats.TakeDamage(damageDealt);
        SoundManager.Instance.PlaySound(tortHeavyImpact, transform, 0.95f);
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
