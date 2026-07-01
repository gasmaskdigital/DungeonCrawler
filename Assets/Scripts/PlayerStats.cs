
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int healthStat = 1;
    public static int strengthStat = 1;
    public static int dexterityStat = 1;
    public static int magicStat = 1;
    public static int enduranceStat = 1;
    public static int playerLevel = 1;
    public static int currentXP = 0;
    public static int requiredXP = 10;

    public static int boostedStrength;
    public static int boostedDexterity;
    public static int boostedMagic;

    public static int maxHealth = 20;
    public static int currentHealth;

    public int earntXP;
    private int randomStat;

    public levelManager levelManager;
    public PHUIManager pHUIManager;

    [Header("Equipment Variables")]
    public static Weapon currentWeapon;
    public static Armour currentHelmet;
    public static Armour currentUpperBody;
    public static Armour currentLowerBody;
    public GameObject[] weaponSockets; // 0=THS, 1=Bow, 2=SpellBook

    public Weapon testWeapon; // this is just used to testing 
    

    public static int currentDefenceTotal;

    public void ResetStats()
    {
        healthStat = 1;
        strengthStat = 1;
        dexterityStat = 1;
        magicStat = 1;
        enduranceStat = 1;
        playerLevel = 1;
        currentXP = 0;
        requiredXP = 10;

        boostedStrength = 0;
        boostedDexterity = 0;
        boostedMagic = 0;
        maxHealth = 20;

        currentWeapon = new Weapon();
        currentHelmet = new Armour();
        currentUpperBody = new Armour();
        currentLowerBody = new Armour();

        currentDefenceTotal = 0;


    }

    private void Awake()
    {
        //currentWeapon = testWeapon; // for testing purposes remove fo main scene
        //UpdateEquipment(); // for testing purposes

        if (levelManager.currentLevel <= 1)
        {
            ResetStats();
            currentHealth = maxHealth;
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToXP(int xpReward)
    {
        currentXP += xpReward;
        if (currentXP > requiredXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        playerLevel++;

        GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManager>().updatePlayerLevel();

        pHUIManager.EnableLevelUpScreen();

        currentXP -= requiredXP;

        requiredXP = requiredXP + (playerLevel * 24);
        
    }

    public void UpdateMaxHealth()
    {
        maxHealth = maxHealth + (healthStat * 5);
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
                boostedStrength += currentWeapon.statBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity +=  currentWeapon.statBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic += currentWeapon.statBoostValue;
                break;
        }

        switch (currentHelmet.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength += currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity += currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic += currentHelmet.StatBoostValue;
                break;
        }

        switch (currentUpperBody.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength += currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity += currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic += currentUpperBody.StatBoostValue;
                break;                                
        }

        switch (currentLowerBody.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength += currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity += currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic += currentLowerBody.StatBoostValue;
                break;
        }

        boostedStrength += strengthStat;
        boostedDexterity += dexterityStat;
        boostedMagic += magicStat;
    }

    private void ResetBoostedStats()
    {
        boostedStrength = 0;
        boostedDexterity = 0;
        boostedMagic = 0;
    }

    private void UpdateWeaponSocket()
    {
        foreach(GameObject c in weaponSockets)
        {
            c.gameObject.SetActive(false);
        }

        switch (currentWeapon.weaponType)
        {
            case WeaponType.TwoHandedSword:
                weaponSockets[0].SetActive(true);
                break;
            case WeaponType.Bow:
                weaponSockets[1].SetActive(true);
                break;
            case WeaponType.FireSpellBook:
                weaponSockets[2].SetActive(true);
                break;
        }
    }

    public void UpdateEquipment()
    {
        currentDefenceTotal = CalculateTotalDefence();
        UpdateBoostedStats();
        UpdateWeaponSocket();
    }

    

    
}