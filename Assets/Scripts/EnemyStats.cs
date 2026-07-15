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

    private GameObject player;
    private PlayerStats playerStats;
    private AINavigation aINavigation;
    private VFXManager vfxManager;
    private levelManager levelManager;


    private Animator enemyAnimator;

    public GameObject floatingText;

    [Header("Sounds")]
    [SerializeField] AudioClip blood;
    [SerializeField] AudioClip Death;
    [SerializeField] AudioClip Pain;

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
        maxHealth = levelManager.currentLevel * maxHealth;
        attack = levelManager.currentLevel * attack;
        defence = levelManager.currentLevel * defence;
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
                SoundManager.Instance.PlaySound(Death, transform);
            }
            else
            {
                enemyAnimator.SetTrigger("DamageReact");
                vfxManager.BlodEffect(transform.position);
                SoundManager.Instance.PlaySound(blood, transform);
                SoundManager.Instance.PlaySound(Pain, transform);
                
            }

            if (floatingText != null)
            {
                ShowFloatingText(damageTaken);
            }

            
        }
    }

    

    void ShowFloatingText(int damageTaken)
    {
       var ft = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        ft.GetComponent<TextMesh>().text = damageTaken.ToString();
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
