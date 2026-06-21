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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerStats = GetComponent<PlayerStats>();
            playerHandler = GetComponent<PlayerHandler>();
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            enemyStats = GetComponent<EnemyStats>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPlayerWeaponType()
    {
        switch (playerStats.currentWeapon.weaponType)
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
                EnemyStats cEnemyStats = GetComponent<EnemyStats>();

                int damageDealt = LightAttackDamage(playerStats.currentWeapon.attackValue, playerStats.boostedStrength, cEnemyStats.defence);

                cEnemyStats.TakeDamage(damageDealt);
            }
            }
        

    }

    public void TwoHandedSwordHeavyAttack()
    {
        Debug.Log("AttackHandler Sword Heavy Attack");

        
            Debug.Log("heavy attack");

            Collider[] colliders = Physics.OverlapSphere(transform.position, tHSHeavyAttckRadius, enemyMask);
            foreach (Collider c in colliders)
            {
                if (c.gameObject.CompareTag("Enemy"))
                {
                EnemyStats cEnemyStats = GetComponent<EnemyStats>();

                
                }
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

        return playerDamage;
    }

    public int HeavyAttackDamage(int weaponAttack, int relevantStat, int enemyDefence)
    {
        int playerValues = weaponAttack + relevantStat;
        playerValues = playerValues * playerValues;

        enemyDefence = enemyDefence * enemyDefence;

        int preDamageValue = playerValues / enemyDefence;

        float floatDamageValue = preDamageValue * 1.2f;
        int playerDamage = (int)floatDamageValue;

        return playerDamage;
    }


}





// Structs and stuffs

public enum StatBoostType
{
    Strength, Dexterity, Magic
}

public enum WeaponType { TwoHandedSword, Bow, FireSpellBook }

public struct Weapon
{
    public string weaponName;
    public int attackValue;    
    public int statBoostValue;
    public StatBoostType statBoost;
    public Mesh weaponModel;
    public WeaponType weaponType;

    public Weapon(string weaponName, int attackValue, int statBoostValue, StatBoostType statBoost, Mesh weaponModel, WeaponType weaponType) 
    {
        this.weaponName = weaponName;
        this.attackValue = attackValue;
        this.statBoostValue = statBoostValue;
        this.statBoost = statBoost;
        this.weaponModel = weaponModel;
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
    

