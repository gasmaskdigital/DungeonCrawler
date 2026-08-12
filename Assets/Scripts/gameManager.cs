using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
 
    [Header("UI References")]
     
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject pnlGameOver;
    [SerializeField] TextMeshProUGUI txtFloor;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] TextMeshProUGUI txtMoney;
    [SerializeField] TextMeshProUGUI txtHealth;
    [SerializeField] TextMeshProUGUI txtStats;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] EffectHandler playerEffects;
    [SerializeField] TextMeshProUGUI txtActiveEffects;

    [Header("Inventory")]

    [SerializeField] InventorySpriteSO inventorySpriteSO;
    [SerializeField] TextMeshProUGUI txtEquipment;
    [SerializeField] TextMeshProUGUI healthCount;
    [SerializeField] TextMeshProUGUI strengthCount;
    [SerializeField] TextMeshProUGUI agilityCount;
    [SerializeField] TextMeshProUGUI manaCount;
    [SerializeField] GameObject currentWeaponSprite;
    [SerializeField] GameObject currentHelmetSprite;
    [SerializeField] GameObject currentUpperBodySprite;
    [SerializeField] GameObject currentLowerBodySprite;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerEffects = GameObject.FindGameObjectWithTag("Player").GetComponent<EffectHandler>();

        updateFloorNumber();
        updatePlayerLevel();
        updateEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        txtHealth.text = PlayerStats.currentHealth + " / " + PlayerStats.maxHealth;
        txtMoney.text = "Money: " + PlayerStats.currency;
        updateStatDisplay();

        if (Input.GetKeyDown(KeyCode.Tab)) updateEquipment();

        if (playerEffects.activeEffects.Count > 0) 
        {
            string activeEffects = "";
            foreach (StatusEffect effect in playerEffects.activeEffects) 
            {
                activeEffects += effect.name + " - " + Mathf.FloorToInt(effect.timeRemaining) + "\r\n";
            }
            
            txtActiveEffects.text = activeEffects;
        }
        else if(txtActiveEffects.text != "") txtActiveEffects.text = "";

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            pnlGameOver.SetActive(true);
            inGameUI.SetActive(false);
        }
    }

    public void updateFloorNumber() 
    {
        txtFloor.text = "Floor: " + levelManager.currentLevel;
    }

    public void updatePlayerLevel() 
    {
        txtLevel.text = "Level: " + PlayerStats.playerLevel;
        
    }

  
    
    public void updateEquipment()
    {
        Armour helmet = PlayerStats.currentHelmet;
        Armour chest = PlayerStats.currentUpperBody;
        Armour legs = PlayerStats.currentLowerBody;
        Weapon weapon = PlayerStats.currentWeapon;

        string currentEquipment = "";

        if (helmet.armourName != null)
        {
            currentHelmetSprite.SetActive(true);
            currentEquipment += "Helmet: " + helmet.armourName + " (" + helmet.armourDefence + " / " + helmet.StatBoostValue + ") " + helmet.statBoost.ToString();
            currentEquipment += "\r\n";
            currentEquipment += "\r\n";
        }
        if (chest.armourName != null)
        {
            currentUpperBodySprite.SetActive(true);
            currentEquipment += "Torso: " + chest.armourName + " (" + chest.armourDefence + " / " + chest.StatBoostValue + ") " + chest.statBoost.ToString();
            currentEquipment += "\r\n";
            currentEquipment += "\r\n";
        }
        if (legs.armourName != null)
        {
            currentLowerBodySprite.SetActive(true);
            currentEquipment += "Legs: " + legs.armourName + " (" + legs.armourDefence + " / " + legs.StatBoostValue + ") " + legs.statBoost.ToString();
            currentEquipment += "\r\n";
            currentEquipment += "\r\n";
        }
        if (weapon.weaponName != null) {
            currentWeaponSprite.SetActive(true);
            currentEquipment += "Weapon: " + weapon.weaponName + " (" + weapon.attackValue + " / " + weapon.statBoostValue + ") " + weapon.statBoost.ToString();
        }

        txtEquipment.text = currentEquipment;

        switch (weapon.weaponType) 
        {
            case WeaponType.TwoHandedSword: 
                {
                    currentWeaponSprite.GetComponent<Image>().sprite = inventorySpriteSO.sword;
                    break;
                }
            case WeaponType.Bow:
                {
                    currentWeaponSprite.GetComponent<Image>().sprite = inventorySpriteSO.bow;
                    break;
                }
            case WeaponType.FireSpellBook:
                {
                    currentWeaponSprite.GetComponent<Image>().sprite = inventorySpriteSO.book;
                    break;
                }
            case WeaponType.Axe:
                {
                    currentWeaponSprite.GetComponent<Image>().sprite = inventorySpriteSO.axe;
                    break;
                }
            case WeaponType.Claw:
                {
                    currentWeaponSprite.GetComponent<Image>().sprite = inventorySpriteSO.claws;
                    break;
                }
            case WeaponType.IceStaff:
                {
                    currentWeaponSprite.GetComponent<Image>().sprite = inventorySpriteSO.staff;
                    break;
                }
        }

        healthCount.text = PlayerStats.healthPotionStack.ToString();
        strengthCount.text = PlayerStats.strengthPotionStack.ToString();
        agilityCount.text = PlayerStats.dexterityPotionStack.ToString();
        manaCount.text = PlayerStats.magicPotionStack.ToString();

        if(PlayerStats.healthPotionStack == 3) healthCount.color = Color.red;
        else healthCount.color = Color.white;
        if (PlayerStats.strengthPotionStack == 3) strengthCount.color = Color.red;
        else strengthCount.color = Color.white;
        if (PlayerStats.dexterityPotionStack == 3) agilityCount.color = Color.red;
        else agilityCount.color = Color.white;
        if (PlayerStats.magicPotionStack == 3) manaCount.color = Color.red;
        else manaCount.color = Color.white;
    }

    public void updateStatDisplay() 
    {
        txtStats.text = "Strength: " + PlayerStats.strengthStat.ToString() + "\r\nDexterity: " + PlayerStats.dexterityStat.ToString() + "\r\nMagic: " + PlayerStats.magicStat.ToString() + "\r\nEndurance: " + PlayerStats.enduranceStat.ToString() + "\r\nVigor: " + PlayerStats.healthStat.ToString();
    }

    public void ToggleGameOver() 
    {
        inGameUI.SetActive(!inGameUI.activeSelf);
        pnlGameOver.SetActive(!pnlGameOver.activeSelf);
    }

    public void resetGame(string scene) 
    {
        playerStats.ResetStats();
        levelManager.currentLevel = 0;
        SceneManager.LoadScene(scene);
    }

    public void loadScene(string scene) 
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }
}
