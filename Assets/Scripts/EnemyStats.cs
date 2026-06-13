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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
