using UnityEngine;

public enum lootType { Weapon, Armour}

public class lootScript : MonoBehaviour
{
    [Header("Loot Parameters")]
    [SerializeField] public lootType lootType;
    [SerializeField] public Armour armour;
    [SerializeField] public Weapon weapon;
    public string lootName;
    public int statValue; // Attack Value or Defence Value for Weapons and Armour respectively
    public int statBoostValue;
    public StatBoostType statBoost;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public ArmourSlot armourSlot;

    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject canvas;
    [SerializeField] bool isPlayerClose;
    [SerializeField] public bool isNewLoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        
        if (isNewLoot)
        {
            statValue = Random.Range(levelManager.currentLevel * 2, levelManager.currentLevel * 3 + 1);
            statBoostValue = Random.Range(levelManager.currentLevel, levelManager.currentLevel * 2 + 1);

            if (lootType == lootType.Weapon)
            {
                statBoost = (StatBoostType)Random.Range(0, 3);
                weapon = new Weapon(lootName, statValue, statBoostValue, statBoost, weaponType);
            }
            if (lootType == lootType.Armour) armour = new Armour(lootName, statValue, statBoostValue, armourSlot, statBoost);
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
                if (lootType == lootType.Weapon)
                {
                    foreach (GameObject weapon in lootHandler.weapons)
                    {
                        if (weapon.GetComponent<lootScript>().lootName == PlayerStats.currentWeapon.weaponName && PlayerStats.currentWeapon.weaponName != null)
                        {
                            GameObject newWeapon = Instantiate(weapon, transform.position, transform.rotation);
                            newWeapon.GetComponent<lootScript>().weapon = PlayerStats.currentWeapon;
                            break;
                        }
                    }
                    PlayerStats.currentWeapon = weapon;
                }
                else if (lootType == lootType.Armour)
                {
                    switch (armourSlot)
                    {
                        case (ArmourSlot.Helmet):
                            foreach (GameObject armour in lootHandler.armour)
                            {
                                if (armour.GetComponent<lootScript>().lootName == PlayerStats.currentHelmet.armourName && PlayerStats.currentHelmet.armourName != null)
                                {
                                    GameObject newHelmet = Instantiate(armour, transform.position, transform.rotation);
                                    newHelmet.GetComponent<lootScript>().armour = PlayerStats.currentHelmet;
                                    break;
                                }
                            }
                            PlayerStats.currentHelmet = armour;
                            break;
                        case (ArmourSlot.UpperBody):
                            foreach (GameObject armour in lootHandler.armour)
                            {
                                if (armour.GetComponent<lootScript>().lootName == PlayerStats.currentUpperBody.armourName && PlayerStats.currentUpperBody.armourName != null)
                                {
                                    GameObject newUpperBody = Instantiate(armour, transform.position, transform.rotation);
                                    newUpperBody.GetComponent<lootScript>().armour = PlayerStats.currentUpperBody;
                                    break;
                                }
                            }
                            PlayerStats.currentUpperBody = armour;
                            break;
                        case (ArmourSlot.Lowerbody):
                            foreach (GameObject armour in lootHandler.armour)
                            {
                                if (armour.GetComponent<lootScript>().lootName == PlayerStats.currentLowerBody.armourName && PlayerStats.currentLowerBody.armourName != null)
                                {
                                    GameObject newLowerBody = Instantiate(armour, transform.position, transform.rotation);
                                    newLowerBody.GetComponent<lootScript>().armour = PlayerStats.currentLowerBody;
                                    break;
                                }
                            }
                            PlayerStats.currentLowerBody = armour;
                            break;
                    }
                }
                //Debug.Log("Equipping: " + lootName);
                playerStats.UpdateEquipment();
                GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManager>().updateEquipment();
                Destroy(gameObject);
            }
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
