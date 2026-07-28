using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject pnlGameOver;
    [SerializeField] TextMeshProUGUI txtFloor;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] TextMeshProUGUI txtHealth;
    [SerializeField] TextMeshProUGUI txtEquipment;
    [SerializeField] TextMeshProUGUI txtStats;
    [SerializeField] PlayerStats playerStats;

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
        txtHealth.text = " " + PlayerStats.currentHealth;
        updateStatDisplay();
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
        txtEquipment.text = "Weapon: " + PlayerStats.currentWeapon.weaponName + " (" + PlayerStats.currentWeapon.attackValue + " / " + PlayerStats.currentWeapon.statBoostValue + ")" +
            "\r\nHelmet: " + PlayerStats.currentHelmet.armourName + " (" + PlayerStats.currentHelmet.armourDefence + " / " + PlayerStats.currentHelmet.StatBoostValue + ")" +
            "\r\nTorso: " + PlayerStats.currentUpperBody.armourName + " (" + PlayerStats.currentUpperBody.armourDefence + " / " + PlayerStats.currentUpperBody.StatBoostValue + ")" +
            "\r\nLegs: " + PlayerStats.currentLowerBody.armourName + " (" + PlayerStats.currentLowerBody.armourDefence + " / " + PlayerStats.currentLowerBody.StatBoostValue + ")";
    }

    public void updateStatDisplay() 
    {
        txtStats.text = "Strength: " + PlayerStats.strengthStat.ToString() + "\r\nDexterity: " + PlayerStats.dexterityStat.ToString() + "\r\nMagic: " + PlayerStats.magicStat.ToString();
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
