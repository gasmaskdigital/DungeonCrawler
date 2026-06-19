using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("References")]
    private PlayerStats playerStats;
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

    private void TwoHandedSwordLightAttack()
    {
        Debug.Log("AttackHandler Sword Light Attack");


    }

    private void TwoHandedSwordHeavyAttack()
    {

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
    

