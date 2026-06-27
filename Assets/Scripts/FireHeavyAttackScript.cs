using UnityEngine;

public class FireHeavyAttackScript : MonoBehaviour
{
    private BoxCollider boxCollider;
    private GameObject player;
    private AttackHandler playerAttackHandler;
    private float speed = 12f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackHandler = player.GetComponent<AttackHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            AINavigation cAINav = other.GetComponent<AINavigation>();
            playerAttackHandler.FireHeavyImpact(enemyStats, cAINav);

        }

        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}

