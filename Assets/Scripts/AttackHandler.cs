using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("References")]
    private PlayerStats playerStats;
    private PlayerHandler playerHandler;
    private EnemyStats enemyStats;
    private EffectHandler effectHandler;
    

    [Header("Player Related")]
    private float tHSLightAttckRadius = 1.5f; // Two Handed Sword Attack Radius
    private float tHSHeavyAttckRadius = 3f; // Two Handed Sword Attack Radius
    public LayerMask enemyMask;
    public AttackType attackType;
    [SerializeField] GameObject lightArrow;
    [SerializeField] GameObject heavyArrow;
    [SerializeField] GameObject lightFire;
    [SerializeField] GameObject heavyFire;
    [SerializeField] GameObject iceLight;
    [SerializeField] GameObject iceHeavy;
    private float knockbackForce = 250f;
    private float axeLightAttackRadius = 1.25f;
    private float axeHeavyAttackRadius = 1.75f;


    [Header("Sounds")]
    [SerializeField] AudioClip swordClash;
    [SerializeField] AudioClip heavyArrowSpawn;
    [SerializeField] AudioClip lightArrowSpawn;
    [SerializeField] AudioClip lightArrowImpact;
    [SerializeField] AudioClip fireLightSpawn;
    [SerializeField] AudioClip fireLightImpact;
    [SerializeField] AudioClip fireWall;
    [SerializeField] AudioClip axeLightImpact;
    [SerializeField] AudioClip axeHeavyImpact;
    [SerializeField] AudioClip clawLightImpact;
    [SerializeField] AudioClip clawHeavyImpact;
    [SerializeField] AudioClip iceLightSpawn;
    [SerializeField] AudioClip iceHeavySpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponent<EffectHandler>() != null) effectHandler = GetComponent<EffectHandler>();

        if (gameObject.CompareTag("Player"))
        {
            playerStats = GetComponent<PlayerStats>();
            playerHandler = GetComponent<PlayerHandler>();
            
        }
        
        if(PlayerStats.currentWeapon.attackValue == 0)
        {
            PlayerStats.currentWeapon.attackValue = 10;
            PlayerStats.currentLowerBody.armourDefence = 10;
            PlayerStats.currentUpperBody.armourDefence = 10;
            PlayerStats.currentHelmet.armourDefence = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPlayerWeaponType()
    {
        switch (PlayerStats.currentWeapon.weaponType)
        {
            case WeaponType.TwoHandedSword:
                if(attackType == AttackType.LightAttack)
                {
                    TwoHandedSwordLightAttack();
                }
                else
                {
                    TwoHandedSwordHeavyAttack();
                }
            break;
            case WeaponType.Bow:
                if(attackType == AttackType.LightAttack)
                {
                    SpawnLightArrow();
                }
                else
                {
                    SpawnHeavyArrow();
                }

                    break;
            case WeaponType.FireSpellBook:

                break;

        }
    }

    public void TwoHandedSwordLightAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, tHSLightAttckRadius, enemyMask);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemyStats cEnemyStats = c.GetComponent<EnemyStats>();
                Debug.Log(cEnemyStats);

                AINavigation cAINav = c.GetComponent<AINavigation>();

                if (cAINav.alive) // && cAINav.canBeDamaged)
                {
                    int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedStrength, cEnemyStats.defence);

                    damageDealt = CheckForEffect(damageDealt, "Strength");
                    if (CheckForCrit())
                    {
                        damageDealt *= 2;
                        cEnemyStats.wasCrit = true;
                    }

                    cEnemyStats.TakeDamage(damageDealt);

                    cAINav.GetKnockBack(knockbackForce, transform.position);

                    SoundManager.Instance.PlaySound(swordClash, transform);
                }
            }
        }
    }

    public void TwoHandedSwordHeavyAttack()
    {
       

            Collider[] colliders = Physics.OverlapSphere(transform.position, tHSHeavyAttckRadius, enemyMask);
            foreach (Collider c in colliders)
            {
                if (c.gameObject.CompareTag("Enemy"))
                {
                EnemyStats cEnemyStats = c.GetComponent<EnemyStats>();
                AINavigation cAINav = c.GetComponent<AINavigation>();

                if (cAINav.alive )//&& cAINav.canBeDamaged)
                {
                    int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedStrength, cEnemyStats.defence);

                    damageDealt = CheckForEffect(damageDealt, "Strength");
                    if (CheckForCrit())
                    {
                        damageDealt *= 2;
                        cEnemyStats.wasCrit = true;
                    }
                    cEnemyStats.TakeDamage(damageDealt);

                    cAINav.GetKnockBack(knockbackForce, transform.position);
                    
                    SoundManager.Instance.PlaySound(swordClash, transform);
                }
                else
                {
                    
                }
                
                }
            }        
    }

    private int CheckForEffect(int damageDealt, string effect) 
    {
        foreach (StatusEffect e in effectHandler.activeEffects)
        {
            if (e.name == effect)
            {
                damageDealt = Mathf.CeilToInt(damageDealt * (1 + 0.25f * e.intensity));
                break;
            }
        }

        return damageDealt;
    }

    private void SpawnLightArrow()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;
        


        Instantiate(lightArrow, spawnPoint, spawnRotation);
        SoundManager.Instance.PlaySound(lightArrowSpawn, transform);
    }

    public void BowLightAttackImpact(EnemyStats enemystats, AINavigation cAiNav)
    {


        if (cAiNav.alive)
        {
            int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemystats.defence);
            damageDealt = CheckForEffect(damageDealt, "Agility");
            if (CheckForCrit())
            {
                damageDealt *= 2;
                enemystats.wasCrit = true;
            }
            enemystats.TakeDamage(damageDealt);

            cAiNav.GetKnockBack(knockbackForce, transform.position);
            SoundManager.Instance.PlaySound(lightArrowImpact, transform);
        }

    }

    private void SpawnHeavyArrow()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;

        Instantiate(heavyArrow, spawnPoint, spawnRotation);
        SoundManager.Instance.PlaySound(heavyArrowSpawn, transform);
    }

    public void BowHeavyAttackImpact(EnemyStats enemyStats, AINavigation cAINav)
    {


        if (cAINav.alive)
        {
            int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemyStats.defence);
            damageDealt = CheckForEffect(damageDealt, "Agility");
            if (CheckForCrit())
            {
                damageDealt *= 2;
                enemyStats.wasCrit = true;
            }
            enemyStats.TakeDamage(damageDealt);
            cAINav.GetKnockBack(knockbackForce, transform.position);
        }
    }

    public void SpawnLightFireball()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;
        Instantiate(lightFire, spawnPoint, spawnRotation);
        SoundManager.Instance.PlaySound(fireLightSpawn, transform);
    }

    public void SpawnHeavyFireWave()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 0.8f;

        quaternion spawnRotation = transform.rotation;

        Instantiate(heavyFire, spawnPoint, spawnRotation);
        SoundManager.Instance.PlaySound(fireWall, transform);
    }

    public void FireHeavyImpact(EnemyStats enemyStats, AINavigation cAINav)
    {


        if (cAINav.alive)
        {
            int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemyStats.defence);
            damageDealt = CheckForEffect(damageDealt, "Mana");
            if (CheckForCrit())
            {
                damageDealt *= 2;
                enemyStats.wasCrit = true;
            }
            enemyStats.TakeDamage(damageDealt);
            cAINav.GetKnockBack(knockbackForce, transform.position);
        }
    }

    public void FireLightAttackImpact(EnemyStats enemyStats, AINavigation cAINav)
    {

        if (cAINav.alive)
        {
            int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedMagic, enemyStats.defence);
            damageDealt = CheckForEffect(damageDealt, "Mana");
            if (CheckForCrit()) 
            {
                damageDealt *= 2;
                enemyStats.wasCrit = true;
            }
            enemyStats.TakeDamage(damageDealt);
            cAINav.GetKnockBack(knockbackForce, transform.position);
            SoundManager.Instance.PlaySound(fireLightImpact, transform);
        }
    }

    public void AxeLightAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

        Collider[] colliders = Physics.OverlapSphere(origin, axeLightAttackRadius, enemyMask);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemyStats cEnemyStats = c.GetComponent<EnemyStats>();
                Debug.Log(cEnemyStats);

                AINavigation cAINav = c.GetComponent<AINavigation>();

                if (cAINav.alive)
                {
                    int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedStrength, cEnemyStats.defence);

                    damageDealt = CheckForEffect(damageDealt, "Strength");
                    if (CheckForCrit())
                    {
                        damageDealt *= 2;
                        cEnemyStats.wasCrit = true;
                    }

                    cEnemyStats.TakeDamage(damageDealt);

                    cAINav.GetKnockBack(knockbackForce, transform.position);

                    SoundManager.Instance.PlaySound(axeLightImpact, transform);
                }


            }
        }
    }

    public void AxeHeavyAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 1.25f;

        Collider[] colliders = Physics.OverlapSphere(origin, axeHeavyAttackRadius, enemyMask);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemyStats cEnemyStats = c.GetComponent<EnemyStats>();
                Debug.Log(cEnemyStats);

                AINavigation cAINav = c.GetComponent<AINavigation>();

                if (cAINav.alive)
                {
                    int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedStrength, cEnemyStats.defence);

                    damageDealt = CheckForEffect(damageDealt, "Strength");
                    if (CheckForCrit())
                    {
                        damageDealt *= 2;
                        cEnemyStats.wasCrit = true;
                    }

                    cEnemyStats.TakeDamage(damageDealt);

                    cAINav.GetKnockBack(knockbackForce, transform.position);

                    SoundManager.Instance.PlaySound(axeHeavyImpact, transform);
                }


            }
        }
    }

    public void ClawLightAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.6f;

        Collider[] colliders = Physics.OverlapSphere(origin, 0.85f, enemyMask);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemyStats cEnemyStats = c.GetComponent<EnemyStats>();
                Debug.Log(cEnemyStats);

                AINavigation cAINav = c.GetComponent<AINavigation>();

                if (cAINav.alive)
                {
                    int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, cEnemyStats.defence);

                    damageDealt = CheckForEffect(damageDealt, "Agility");
                    if (CheckForCrit())
                    {
                        damageDealt *= 2;
                        cEnemyStats.wasCrit = true;
                    }

                    cEnemyStats.TakeDamage(damageDealt);

                    cAINav.GetKnockBack(knockbackForce, transform.position);

                    SoundManager.Instance.PlaySound(axeLightImpact, transform);
                }


            }
        }
    }

    public void ClawHeavyAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.8f;

        Collider[] colliders = Physics.OverlapSphere(origin, 1.25f, enemyMask);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemyStats cEnemyStats = c.GetComponent<EnemyStats>();
                Debug.Log(cEnemyStats);

                AINavigation cAINav = c.GetComponent<AINavigation>();

                if (cAINav.alive)
                {
                    int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedStrength, cEnemyStats.defence);

                    damageDealt = CheckForEffect(damageDealt, "Agility");
                    if (CheckForCrit())
                    {
                        damageDealt *= 2;
                        cEnemyStats.wasCrit = true;
                    }

                    cEnemyStats.TakeDamage(damageDealt);

                    cAINav.GetKnockBack(knockbackForce, transform.position);

                    SoundManager.Instance.PlaySound(clawHeavyImpact, transform);
                }


            }
        }
    }

    public void IceLightAttackSpawn()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 0.25f;
        spawnPoint.z += 0.4f;

        quaternion spawnRotation = transform.rotation;
        Instantiate(iceLight, spawnPoint, spawnRotation);
        SoundManager.Instance.PlaySound(iceLightSpawn, transform);
    }

    public void IceLightImpact(EnemyStats enemystats, AINavigation cAiNav)
    {
        if (cAiNav.alive)
        {
            int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedMagic, enemyStats.defence);
            damageDealt = CheckForEffect(damageDealt, "Mana");
            if (CheckForCrit())
            {
                damageDealt *= 2;
                enemyStats.wasCrit = true;
            }
            enemyStats.TakeDamage(damageDealt);
            cAiNav.GetKnockBack(knockbackForce, transform.position);
            
        }
    }

    public void IceHeavyAttackSpawn()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 0.1f;
        spawnPoint.z += 4.5f;

        quaternion spawnRotation = transform.rotation;
        Instantiate(iceHeavy, spawnPoint, spawnRotation);
        SoundManager.Instance.PlaySound(iceHeavySpawn, transform);
    }

    public void IceHeavyImpact(EnemyStats enemystats, AINavigation cAiNav)
    {
        if (cAiNav.alive)
        {
            int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemyStats.defence);
            damageDealt = CheckForEffect(damageDealt, "Mana");
            if (CheckForCrit())
            {
                damageDealt *= 2;
                enemyStats.wasCrit = true;
            }
            enemyStats.TakeDamage(damageDealt);
            cAiNav.GetKnockBack(knockbackForce, transform.position);
        }
    }

    public int LightAttackDamage(int weaponAttack, int relevantStat, int enemyDefence)
    {
        int playerValues = weaponAttack + relevantStat;
        int preDamageValues = playerValues - enemyDefence;
        int damageDealt = Mathf.CeilToInt(preDamageValues * 1.2f);

        return damageDealt;
        
    }

    public int HeavyAttackDamage(int weaponAttack, int relevantStat, int enemyDefence)
    {
        int playerValues = weaponAttack + relevantStat;
        int preDamageValues = playerValues - enemyDefence;
        int damageDealt = Mathf.CeilToInt(preDamageValues * 1.5f);

        return damageDealt;
    }

    private bool CheckForCrit()
    {
        int critChance = UnityEngine.Random.Range(1, 21);
        if (critChance == 20) return true;
        else return false;

        
    }


}





// Structs and stuffs

public enum StatBoostType
{
    Strength, Dexterity, Magic, Endurance, Health
}

public enum WeaponType
{
    TwoHandedSword, Bow, FireSpellBook, Axe, Claw, IceStaff
}

[System.Serializable]
public struct Weapon
{
    public string weaponName;
    public int attackValue;    
    public int statBoostValue;
    public StatBoostType statBoost;
    public WeaponType weaponType;

    public Weapon(string weaponName, int attackValue, int statBoostValue, StatBoostType statBoost, WeaponType weaponType)
    {
        this.weaponName = weaponName;
        this.attackValue = attackValue;
        this.statBoostValue = statBoostValue;
        this.statBoost = statBoost;
        this.weaponType = weaponType;
    }
}

public enum ArmourSlot
{
    Helmet, UpperBody, Lowerbody
}

public enum AttackType
{
    LightAttack,HeavyAttack
}



public struct Armour
{
    public string armourName;
    public int armourDefence;
    public int StatBoostValue;
    public ArmourSlot armourSlot;
    public StatBoostType statBoost;

    public Armour(string armourName, int armourDefence, int StatBoostValue, ArmourSlot armourSlot, StatBoostType statBoost) 
    {
        this.armourName = armourName;
        this.armourDefence = armourDefence;
        this.StatBoostValue = StatBoostValue;
        this.armourSlot = armourSlot;
        this.statBoost = statBoost;
    }
}
    

