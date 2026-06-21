using UnityEngine;

public class lootScript : MonoBehaviour
{
    [Header("Weapon Parameters")] 
    [SerializeField] public Weapon weapon;
    public string weaponName;
    public int attackValue;
    public int statBoostValue;
    public StatBoostType statBoost;
    public Mesh weaponModel;

    [Header("References")]
    [SerializeField] PlayerHandler playerHandler;
    [SerializeField] GameObject canvas;
    [SerializeField] bool isPlayerClose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapon = new Weapon(weaponName, attackValue, statBoostValue, statBoost, weaponModel);
        playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            foreach (GameObject weapon in lootHandler.weapons) 
            {
                if (weapon.GetComponent<lootScript>().weaponName == playerHandler.curWeapon.weaponName && playerHandler.curWeapon.weaponName != null) 
                {
                    Instantiate(weapon,transform.position,Quaternion.identity);
                }
            }
            playerHandler.curWeapon = weapon;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerHandler.gameObject)
        { 
            isPlayerClose = true; 
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerHandler.gameObject)
        {
            isPlayerClose = false;
            canvas.SetActive(false);
        }
    }
}
