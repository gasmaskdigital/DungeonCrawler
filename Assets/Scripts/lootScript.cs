using UnityEngine;

public enum lootType { Weapon, Armour}

public class lootScript : MonoBehaviour
{
    [Header("Loot Parameters")]
    [SerializeField] public lootType lootType;
    [SerializeField] public Armour armour;
    [SerializeField] public Weapon weapon;
    public string lootName;
    public int statValue; // Attack Value or Defence Value for Weapons and Amour respectively
    public int statBoostValue;
    public StatBoostType statBoost;
    public Mesh lootModel;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public ArmourSlot armourSlot;

    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject canvas;
    [SerializeField] bool isPlayerClose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(lootType == lootType.Weapon) weapon = new Weapon(lootName, statValue, statBoostValue, statBoost, lootModel, weaponType);
        if(lootType == lootType.Armour) armour = new Armour(lootName, statValue, statBoostValue, armourSlot, statBoost);
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            if (lootType == lootType.Weapon)
            {
                foreach (GameObject weapon in lootHandler.weapons)
                {
                    if (weapon.GetComponent<lootScript>().lootName == playerStats.currentWeapon.weaponName && playerStats.currentWeapon.weaponName != null)
                    {
                        Instantiate(weapon, transform.position, transform.rotation);
                    }
                }
                playerStats.currentWeapon = weapon;
            }
            else if (lootType == lootType.Armour) 
            {
                switch (armourSlot)
                {
                    case (ArmourSlot.Helmet):
                        foreach (GameObject armour in lootHandler.armour)
                        {
                            if (armour.GetComponent<lootScript>().lootName == playerStats.currentHelmet.armourName && playerStats.currentHelmet.armourName != null)
                            {
                                Instantiate(armour, transform.position, transform.rotation);
                            }
                        }
                        playerStats.currentHelmet = armour;
                        break;
                    case (ArmourSlot.UpperBody):
                        foreach (GameObject armour in lootHandler.armour)
                        {
                            if (armour.GetComponent<lootScript>().lootName == playerStats.currentUpperBody.armourName && playerStats.currentUpperBody.armourName != null)
                            {
                                Instantiate(armour, transform.position, transform.rotation);
                            }
                        }
                        playerStats.currentUpperBody = armour;
                        break;
                    case (ArmourSlot.Lowerbody):
                        foreach (GameObject armour in lootHandler.armour)
                        {
                            if (armour.GetComponent<lootScript>().lootName == playerStats.currentLowerBody.armourName && playerStats.currentLowerBody.armourName != null)
                            {
                                Instantiate(armour, transform.position, transform.rotation);
                            }
                        }
                        playerStats.currentLowerBody = armour;
                        break;
                }
            }
            Debug.Log("Equipping: " + lootName);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerStats.gameObject)
        { 
            isPlayerClose = true; 
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerStats.gameObject)
        {
            isPlayerClose = false;
            canvas.SetActive(false);
        }
    }
}
