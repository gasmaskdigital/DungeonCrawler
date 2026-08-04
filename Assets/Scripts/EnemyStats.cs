using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int attack;
    public int maxHealth;
    public int currentHealth;
    public int defence;
    public int xpReward;
    public float moveSpeed;
    public float attackRange;
    public string enemyName;
    public float enemyAttackRadius;
    public bool canBeDamaged = true;
    public bool wasCrit = false;

    private GameObject player;
    private PlayerStats playerStats;
    private AINavigation aINavigation;
    private VFXManager vfxManager;
    private levelManager levelManager;


    private Animator enemyAnimator;

    public GameObject floatingText;
    public GameObject[] lootDrops;
    private int lootIndex;

    [Header("Sounds")]
    [SerializeField] AudioClip blood;
    [SerializeField] AudioClip Death;
    [SerializeField] AudioClip Pain;
    [SerializeField] AudioClip critSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {               
        enemyAnimator = GetComponentInChildren<Animator>();
        aINavigation = GetComponent<AINavigation>();
        vfxManager = FindAnyObjectByType<VFXManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        levelManager = FindAnyObjectByType<levelManager>();

        StatScaling();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StatScaling()
    {
        maxHealth += (levelManager.currentLevel * 5);
        attack += (levelManager.currentLevel * 2);
        defence += (levelManager.currentLevel * 2);
        xpReward += (levelManager.currentLevel * 2);
    }

    public void TakeDamage(int damageTaken)
    {
        if (canBeDamaged)
        {
            currentHealth = currentHealth - damageTaken;

            
            if(currentHealth <= 0)
            {
                playerStats.AddToXP(xpReward);
                enemyAnimator.SetTrigger("Death");
                aINavigation.alive = false;
                aINavigation.DisableNavAndCollider();
                vfxManager.DeathEffect(transform.position);
                SoundManager.Instance.PlaySound(Death, transform, 1f);
                CheckForLoot();
            }
            else
            {
                enemyAnimator.SetTrigger("DamageReact");
                vfxManager.BlodEffect(transform.position);
                SoundManager.Instance.PlaySound(blood, transform, 0.75f);
                SoundManager.Instance.PlaySound(Pain, transform, 0.85f);
                
            }

            if (floatingText != null)
            {
                ShowFloatingText(damageTaken);
            }

            
        }
    }

    private void CheckForLoot()
    {
        int dropChance = Random.Range(0, 10);
        if(dropChance > 8)
        {
            Vector3 spawnPos = transform.position;
            Instantiate(lootDrops[Random.Range(0, lootDrops.Length)], spawnPos, Quaternion.identity).GetComponent<lootScript>().isNewLoot = true;
        }
       
    }

    void ShowFloatingText(int damageTaken)
    {
       var ft = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        ft.GetComponent<TextMesh>().text = damageTaken.ToString();

        if (wasCrit)
        {
            ft.GetComponent<TextMesh>().color = Color.yellow;
            SoundManager.Instance.PlaySound(critSound, transform, 1.1f);
        }
        else
        {
            ft.GetComponent<TextMesh>().color = Color.white;
        }

        wasCrit = false;
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
        Debug.Log("Death");
    }


}
