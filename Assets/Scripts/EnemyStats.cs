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

    private Animator enemyAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth = currentHealth - damageTaken;

        enemyAnimator.SetTrigger("DamageReact");

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
