using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private PlayerStats playerStats;
    private EnemyStats enemyStats;

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
}

public enum StatBoostType
{
    Strength, Dexterity, Magic
}
public struct Weapon
{
    public string weaponName;
    public int attackValue;    
    public int statBoostValue;
    public StatBoostType statBoost;
}

public enum ArmourSlot
{
    Helmet, UpperBody, Lowerbody
}

public struct Armour
{
    public string armourName;
    public int armourDefence;
    public int StatBoostValue;
    public ArmourSlot armourSlot;
    public StatBoostType statBoost;
}
