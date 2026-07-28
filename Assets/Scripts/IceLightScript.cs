using UnityEngine;

public class IceLightScript : MonoBehaviour
{
    private GameObject player;
    private AttackHandler playerAttackHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackHandler = player.GetComponent<AttackHandler>();
        Destroy(gameObject, 1.75f);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyStats cEnemyStats = other.GetComponent<EnemyStats>();
                AINavigation cAINav = other.GetComponent<AINavigation>();

                if (cEnemyStats == null || cAINav == null)
                {
                    Debug.LogWarning($"Missing EnemyStats or AINavigation on {other.name}");
                    return;
                }

                playerAttackHandler.IceLightImpact(cEnemyStats, cAINav);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
