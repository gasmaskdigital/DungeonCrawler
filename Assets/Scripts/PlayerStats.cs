
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthStat = 1;
    public int strengthStat = 1;
    public int dexterityStat = 1;
    public int magicStat = 1;
    public int enduranceStat = 1;
    public int playerLevel = 1;
    public int currentXP = 0;
    public int requiredXP = 150;

    public int boostedStrength;
    public int boostedDexterity;
    public int boostedMagic;

    public int maxHealth = 100;
    public int currentHealth;

    public int earntXP;
    private int randomStat;

    [Header("Equipment Variables")]
    public Weapon currentWeapon;
    public Armour currentHelmet;
    public Armour currentUpperBody;
    public Armour currentLowerBody;
    

    public int currentDefenceTotal;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(healthStat < 1)
        {
            currentHealth = maxHealth;
        }
        else
        {
            maxHealth = maxHealth * (healthStat * 5);
            currentHealth = maxHealth;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToXP()
    {
        currentXP += earntXP;
        if (currentXP > requiredXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        randomStat = Random.Range(0, 4);

        switch (randomStat)
        {
            case 0:
                healthStat++;
                break;
            case 1:
                strengthStat++;
                break;
            case 2:
                dexterityStat++;
                break;
            case 3:
                magicStat++;
                break;
            case 4:
                enduranceStat++;
                break;
        }

        currentXP = 0;
        requiredXP = requiredXP + 125;
        playerLevel++;
    }

    public int CalculateTotalDefence()
    {
        int totalDefence = currentHelmet.armourDefence + currentUpperBody.armourDefence + currentLowerBody.armourDefence;
        return totalDefence;
    }

    public void UpdateBoostedStats()
    {
        ResetBoostedStats();

        switch (currentWeapon.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = strengthStat + currentWeapon.statBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = dexterityStat + currentWeapon.statBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = magicStat + currentWeapon.statBoostValue;
                break;
        }

        switch (currentHelmet.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = strengthStat + currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = dexterityStat + currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = magicStat + currentHelmet.StatBoostValue;
                break;
        }

        switch (currentUpperBody.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = strengthStat + currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = dexterityStat + currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = magicStat + currentUpperBody.StatBoostValue;
                break;                                
        }

        switch (currentLowerBody.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = strengthStat + currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = dexterityStat + currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = magicStat + currentLowerBody.StatBoostValue;
                break;
        }
    }

    private void ResetBoostedStats()
    {
        boostedStrength = 0;
        boostedDexterity = 0;
        boostedMagic = 0;
    }

    public void UpdateEquipment()
    {
        currentDefenceTotal = CalculateTotalDefence();
        UpdateBoostedStats();
    }

    
}