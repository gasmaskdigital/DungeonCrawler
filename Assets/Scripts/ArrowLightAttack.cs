using UnityEngine;

public class ArrowLightAttack : MonoBehaviour
{
    private SphereCollider SphereCollider;
    private float speed = 15f;
    private GameObject player;
    private AttackHandler playerAttackHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackHandler = player.GetComponent<AttackHandler>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Arrow hitting " + other.gameObject.name);

        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Arrow hit " + other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            EnemyStats cEnemyStats = other.GetComponent<EnemyStats>();
            AINavigation cAINav = other.GetComponent<AINavigation>();
            playerAttackHandler.BowLightAttackImpact(cEnemyStats, cAINav);
            Destroy(gameObject);
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
