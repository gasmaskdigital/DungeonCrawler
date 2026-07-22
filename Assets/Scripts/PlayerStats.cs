
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerHandler playerHandler;
    private VFXManager vfxManager;

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
    public static int boostedEndurance;
    public static int boostedHealth;

    public static int maxHealth = 50;
    public static int currentHealth;

    public int earntXP;
    private int randomStat;

    public levelManager levelManager;
    public PHUIManager pHUIManager;
    public GameObject floatingText;

    [Header("Equipment Variables")]
    public static Weapon currentWeapon;
    public static Armour currentHelmet;
    public static Armour currentUpperBody;
    public static Armour currentLowerBody;
    public GameObject[] weaponSockets; // 0=THS, 1=Bow, 2=SpellBook 3=Axe 4+5=Claw Socket

    public Weapon testWeapon; // this is just used to testing 


    public static int currentDefenceTotal;

    [Header("Sounds")]
    [SerializeField] AudioClip blood;
    [SerializeField] AudioClip playerPain;
    [SerializeField] AudioClip playerDeath;



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
        boostedEndurance = 0;
        boostedHealth = 0;
        maxHealth = 50;

        currentWeapon = new Weapon();
        currentHelmet = new Armour();
        currentUpperBody = new Armour();
        currentLowerBody = new Armour();

        currentDefenceTotal = 0;


    }

    private void Awake()
    {
       // currentWeapon = testWeapon; // used for testing new weapons remove when not needed

        UpdateWeaponSocket();
        
        //UpdateEquipment(); 

        if (levelManager.currentLevel <= 1)
        {
            ResetStats();
            currentHealth = maxHealth;
        }

        

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHandler = GetComponent<PlayerHandler>();
        vfxManager = FindAnyObjectByType<VFXManager>();

        if (healthStat < 1)
        {
            currentHealth = maxHealth;
        }

       

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
        maxHealth += 4;
        currentHealth += 4;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManager>().updatePlayerLevel();

        pHUIManager.EnableLevelUpScreen();

        currentXP -= requiredXP;

        requiredXP = playerLevel * 24;

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
        switch (currentWeapon.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = currentWeapon.statBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = currentWeapon.statBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = currentWeapon.statBoostValue;
                break;
        }

        switch (currentHelmet.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Endurance:
                boostedEndurance = currentHelmet.StatBoostValue;
                break;
            case StatBoostType.Health:
                boostedHealth = currentHelmet.StatBoostValue;
                break;
        }

        switch (currentUpperBody.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Endurance:
                boostedEndurance = currentUpperBody.StatBoostValue;
                break;
            case StatBoostType.Health:
                boostedHealth = currentUpperBody.StatBoostValue;
                break;
        }

        switch (currentLowerBody.statBoost)
        {
            case StatBoostType.Strength:
                boostedStrength = currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Dexterity:
                boostedDexterity = currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Magic:
                boostedMagic = currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Endurance:
                boostedEndurance = currentLowerBody.StatBoostValue;
                break;
            case StatBoostType.Health:
                boostedHealth = currentLowerBody.StatBoostValue;
                break;
        }

        boostedStrength += strengthStat;
        boostedDexterity += dexterityStat;
        boostedMagic += magicStat;
        boostedEndurance += enduranceStat;
        boostedHealth += healthStat;
    }

    private void UpdateWeaponSocket()
    {
        foreach (GameObject c in weaponSockets)
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
            case WeaponType.Axe:
                weaponSockets[3].SetActive(true);
                break;
        }
    }

    public void UpdateEquipment()
    {
        currentDefenceTotal = CalculateTotalDefence();
        UpdateBoostedStats();
        UpdateWeaponSocket();
    }

    public void TakeDamage(int damageDealt)
    {
        if (playerHandler.canBeDamaged)
        {
            currentHealth -= damageDealt;
            if (currentHealth <= 0)
            {
                PlayerDeath();
                playerHandler.DeathTrigger();
                vfxManager.DeathEffect(transform.position);
                SoundManager.Instance.PlaySound(playerDeath, transform);
            }
            else
            {
                playerHandler.DamagedTrigger();
                vfxManager.BlodEffect(transform.position);
                SoundManager.Instance.PlaySound(blood, transform);
                SoundManager.Instance.PlaySound(playerPain, transform);

            }

            if (floatingText != null)
            {
                ShowFloatingText(damageDealt);
            }
        }
    }

    private void PlayerDeath()
    {
        

        AINavigation.playerAlive = false;
              

    }

    void ShowFloatingText(int damageTaken)
    {
        var ft = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        ft.GetComponent<TextMesh>().text = damageTaken.ToString();
        ft.GetComponent<TextMesh>().color = Color.red;
    }


}
    
