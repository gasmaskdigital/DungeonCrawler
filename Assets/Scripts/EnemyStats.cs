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

    private Animator enemyAnimator;

    public GameObject floatingText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        enemyAnimator = GetComponentInChildren<Animator>();
        aINavigation = GetComponent<AINavigation>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

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
            }
            else
            {
                enemyAnimator.SetTrigger("DamageReact");
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
