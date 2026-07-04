using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("References")]
    private PlayerStats playerStats;
    private PlayerHandler playerHandler;
    private EnemyStats enemyStats;

    [Header("Player Related")]
    private float tHSLightAttckRadius = 1.5f; // Two Handed Sword Attack Radius
    private float tHSHeavyAttckRadius = 3f; // Two Handed Sword Attack Radius
    public LayerMask enemyMask;
    public AttackType attackType;
    [SerializeField] GameObject lightArrow;
    [SerializeField] GameObject heavyArrow;
    [SerializeField] GameObject lightFire;
    [SerializeField] GameObject heavyFire;
    private float knockbackForce = 250f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerStats = GetComponent<PlayerStats>();
            playerHandler = GetComponent<PlayerHandler>();
            Debug.Log("Player components");
        }
        
        if(PlayerStats.currentWeapon.attackValue == 0)
        {
            PlayerStats.currentWeapon.attackValue = 10;
            PlayerStats.currentLowerBody.armourDefence = 10;
            PlayerStats.currentLowerBody.armourDefence = 10;
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
        Debug.Log("AttackHandler Sword Light Attack");
        
            Debug.Log("Light Attack");

            Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 0.75f;

            Collider[] colliders = Physics.OverlapSphere(origin, tHSLightAttckRadius, enemyMask);
            Debug.Log("Overlap");
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

                    cEnemyStats.TakeDamage(damageDealt);

                    
                    cAINav.GetKnockBack(knockbackForce, transform.position);
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

                if (cAINav.alive)
                {
                    int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedStrength, cEnemyStats.defence);

                    cEnemyStats.TakeDamage(damageDealt);


                    cAINav.GetKnockBack(knockbackForce, transform.position);

                    Debug.Log("Damage Dealt " + damageDealt);
                }
                else
                {
                    Debug.Log("Hit Dead Enemy");
                }
                
                }
            }        
    }

    private void SpawnLightArrow()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;
        


        Instantiate(lightArrow, spawnPoint, spawnRotation);
    }

    public void BowLightAttackImpact(EnemyStats enemystats, AINavigation cAiNav)
    {


        if (cAiNav.alive)
        {
            int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemystats.defence);
            enemystats.TakeDamage(damageDealt);

            cAiNav.GetKnockBack(knockbackForce, transform.position);
        }

    }

    private void SpawnHeavyArrow()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        quaternion spawnRotation = transform.rotation;

        Instantiate(heavyArrow, spawnPoint, spawnRotation);
    }

    public void BowHeavyAttackImpact(EnemyStats enemyStats, AINavigation cAINav)
    {


        if (cAINav.alive)
        {
            int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemyStats.defence);
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
    }

    public void SpawnHeavyFireWave()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 0.8f;

        quaternion spawnRotation = transform.rotation;

        Instantiate(heavyFire, spawnPoint, spawnRotation);
    }

    public void FireHeavyImpact(EnemyStats enemyStats, AINavigation cAINav)
    {


        if (cAINav.alive)
        {
            int damageDealt = HeavyAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedDexterity, enemyStats.defence);
            enemyStats.TakeDamage(damageDealt);
            cAINav.GetKnockBack(knockbackForce, transform.position);
        }
    }

    public void FireLightAttackImpact(EnemyStats enemyStats, AINavigation cAINav)
    {

        if (cAINav.alive)
        {
            int damageDealt = LightAttackDamage(PlayerStats.currentWeapon.attackValue, PlayerStats.boostedMagic, enemyStats.defence);


            enemyStats.TakeDamage(damageDealt);
            cAINav.GetKnockBack(knockbackForce, transform.position);
        }
    }

    public int LightAttackDamage(int weaponAttack, int relevantStat, int enemyDefence)
    {
        int playerValues = weaponAttack + relevantStat;
        playerValues = playerValues * playerValues;

        enemyDefence = enemyDefence * enemyDefence;

        int preDamageValue = playerValues / enemyDefence;

        float floatDamageValue = preDamageValue * 1.2f;
        int playerDamage = (int)floatDamageValue;

        if(playerDamage <= 0)
        {
            playerDamage += 1;
        }

        return playerDamage;
    }

    public int HeavyAttackDamage(int weaponAttack, int relevantStat, int enemyDefence)
    {
        int playerValues = weaponAttack + relevantStat;
        playerValues = playerValues * playerValues;

        enemyDefence = enemyDefence * enemyDefence;

        int preDamageValue = playerValues / enemyDefence;

        float floatDamageValue = preDamageValue * 1.5f;
        int playerDamage = (int)floatDamageValue;

        if (playerDamage <= 0)
        {
            playerDamage += 2;
        }

        return playerDamage;
    }

   


}





// Structs and stuffs

public enum StatBoostType
{
    Strength, Dexterity, Magic
}

public enum WeaponType
{
    TwoHandedSword, Bow, FireSpellBook
}

[System.Serializable]
public struct Weapon
{
    public string weaponName;
    public int attackValue;    
    public int statBoostValue;
    public StatBoostType statBoost;
    public Mesh weaponModel;
    public WeaponType weaponType;
    
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
}
    

