using System;
using UnityEngine;

[Serializable]
public enum LootType { Weapon, Armour, Potion, Money} // Weapon = 0, Armour = 1, Potion = 2, Money = 3

public class lootScript : MonoBehaviour
{
    [Header("Loot Parameters")]
    [SerializeField] public LootType lootType;
    [SerializeField] public Armour armour;
    [SerializeField] public Weapon weapon;
    [SerializeField] public StatusEffect effect;
    public string lootName;
    public int statValue; // Attack Value or Defence Value for Weapons and Armour respectively
    public int statBoostValue;
    public StatBoostType statBoost;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public ArmourSlot armourSlot;
    [SerializeField] bool pickedUp;

    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] gameManager controller;
    [SerializeField] LootSO allLoot;
    [SerializeField] GameObject canvas;
    [SerializeField] bool isPlayerClose;
    [SerializeField] public bool isNewLoot;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManager>();
        pickedUp = false;

        if (isNewLoot && lootType != LootType.Potion)
        {
            int averageValue = 10 + levelManager.currentLevel * 2;
            int averageBoost = levelManager.currentLevel * 2;

            statValue = UnityEngine.Random.Range(averageValue - levelManager.currentLevel, averageValue + levelManager.currentLevel + 1);
            statBoostValue = UnityEngine.Random.Range(averageBoost - levelManager.currentLevel, averageBoost + levelManager.currentLevel + 1);

            if (lootType == LootType.Weapon) weapon = new Weapon(lootName, statValue, statBoostValue, statBoost, weaponType);
            if (lootType == LootType.Armour)
            {
                statBoost = (StatBoostType)UnityEngine.Random.Range(0, 5);
                armour = new Armour(lootName, statValue, statBoostValue, armourSlot, statBoost);
            }
        }
        else
        {
            switch (lootType)
            {
                case (LootType.Potion):
                    {
                        effect.intensity = Mathf.CeilToInt(levelManager.currentLevel / 4f);
                        if (effect.duration <= 0) effect.intensity *= 10;
                        break;
                    }
                case (LootType.Money):
                    {
                        statValue = UnityEngine.Random.Range(levelManager.currentLevel * 5, levelManager.currentLevel * 10);
                        break;
                    }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose)
        {
            canvas.transform.rotation = Camera.main.transform.rotation;

            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (lootType)
                {
                    case (LootType.Weapon):
                        pickupWeapon();
                        break;
                    case (LootType.Armour):
                        pickupArmour();
                        break;
                    case (LootType.Potion):
                        pickupPotion();
                        break;
                    case (LootType.Money):
                        gameManager.playerMoney += statValue;
                        break;
                }
                
                playerStats.UpdateEquipment();
                controller.updateEquipment();

                if (pickedUp)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void pickupWeapon()
    {
        foreach (Loot weapon in allLoot.lootList)
        {
            if (weapon.name == PlayerStats.currentWeapon.weaponName && PlayerStats.currentWeapon.weaponName != null)
            {
                GameObject newWeapon = Instantiate(weapon.prefab, transform.position, transform.rotation);
                newWeapon.GetComponent<lootScript>().weapon = PlayerStats.currentWeapon;
                break;
            }
        }
        PlayerStats.currentWeapon = weapon;
        pickedUp = true;
    }

    private void pickupArmour()
    {
        switch (armourSlot)
        {
            case (ArmourSlot.Helmet):
                foreach (Loot armour in allLoot.lootList)
                {
                    if (armour.name == PlayerStats.currentHelmet.armourName && PlayerStats.currentHelmet.armourName != null)
                    {
                        GameObject newHelmet = Instantiate(armour.prefab, transform.position, transform.rotation);
                        newHelmet.GetComponent<lootScript>().armour = PlayerStats.currentHelmet;
                        break;
                    }
                }
                PlayerStats.currentHelmet = armour;
                break;

            case (ArmourSlot.UpperBody):
                foreach (Loot armour in allLoot.lootList)
                {
                    if (armour.name == PlayerStats.currentUpperBody.armourName && PlayerStats.currentUpperBody.armourName != null)
                    {
                        GameObject newUpperBody = Instantiate(armour.prefab, transform.position, transform.rotation);
                        newUpperBody.GetComponent<lootScript>().armour = PlayerStats.currentUpperBody;
                        break;
                    }
                }
                PlayerStats.currentUpperBody = armour;
                break;

            case (ArmourSlot.Lowerbody):
                foreach (Loot armour in allLoot.lootList)
                {
                    if (armour.name == PlayerStats.currentLowerBody.armourName && PlayerStats.currentLowerBody.armourName != null)
                    {
                        GameObject newLowerBody = Instantiate(armour.prefab, transform.position, transform.rotation);
                        newLowerBody.GetComponent<lootScript>().armour = PlayerStats.currentLowerBody;
                        break;
                    }
                }
                PlayerStats.currentLowerBody = armour;
                break;
        }
        pickedUp = true;
    }

    private void pickupPotion() 
    {
        int potionStackLimit = 3;
        switch (lootName)
        {
            case "HealthPotion":
                if (PlayerStats.healthPotionStack < potionStackLimit)
                {
                    PlayerStats.healthPotionStack++;
                    pickedUp = true;
                }
                break;
            case "StrengthPotion":
                if (PlayerStats.strengthPotionStack < potionStackLimit)
                {
                    PlayerStats.strengthPotionStack++;
                    pickedUp = true;
                }
                break;
            case "AgilityPotion":
                if (PlayerStats.dexterityPotionStack < potionStackLimit)
                {
                    PlayerStats.dexterityPotionStack++;
                    pickedUp = true;
                }
                //Debug.Log("dex potions " + PlayerStats.dexterityPotionStack);
                break;
            case "ManaPotion":
                if (PlayerStats.magicPotionStack < potionStackLimit)
                {
                    PlayerStats.magicPotionStack++;
                    pickedUp = true;
                }
                //Debug.Log("magic potions " + PlayerStats.magicPotionStack);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerStats.gameObject && !other.isTrigger)
        { 
            isPlayerClose = true; 
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerStats.gameObject && !other.isTrigger)
        {
            isPlayerClose = false;
            canvas.SetActive(false);
        }
    }
}
