using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static int playerMoney;
    

    [Header("UI References")]
     
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject pnlGameOver;
    [SerializeField] TextMeshProUGUI txtFloor;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] TextMeshProUGUI txtMoney;
    [SerializeField] TextMeshProUGUI txtHealth;
    [SerializeField] TextMeshProUGUI txtStats;
    [SerializeField] PlayerStats playerStats;

    [Header("Inventory")]

    [SerializeField] InventorySpriteSO inventorySpriteSO;
    [SerializeField] TextMeshProUGUI txtEquipment;
    [SerializeField] GameObject currentWeaponSprite;
    [SerializeField] GameObject currentHelmetSprite;
    [SerializeField] GameObject currentUpperBodySprite;
    [SerializeField] GameObject currentLowerBodySprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        updateFloorNumber();
        updatePlayerLevel();
        updateEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        txtHealth.text = PlayerStats.currentHealth + " / " + PlayerStats.maxHealth;
        txtMoney.text = "Money: " + playerMoney;
        updateStatDisplay();

        if (Input.GetKeyDown(KeyCode.Tab)) updateEquipment();
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

    public void resetGame() 
    {
        playerStats.ResetStats();
        levelManager.currentLevel = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
