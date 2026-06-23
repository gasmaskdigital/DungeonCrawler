using UnityEngine;

public class HeavyArrow : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private float speed = 20f;
    private GameObject player;
    private AttackHandler playerAttackHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
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
            EnemyStats cEnemyStats = other.GetComponent<EnemyStats>();
            playerAttackHandler.BowHeavyAttackImpact(cEnemyStats);
        }

        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
