using UnityEngine;

public class HeavyArrow : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private float speed = 20f;
    private GameObject player;
    private AttackHandler playerAttackHandler;
    Rigidbody myRigidBody;

    [Header("Sounds")]
    [SerializeField] AudioClip heavyArrowImpact;
    [SerializeField] AudioClip arrowBreak;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackHandler = player.GetComponent<AttackHandler>();
        
        myRigidBody = GetComponent<Rigidbody>();

        myRigidBody.AddForce(transform.forward * speed, ForceMode.Impulse);
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
            AINavigation cAiNav = other.GetComponent<AINavigation>();
            playerAttackHandler.BowHeavyAttackImpact(cEnemyStats, cAiNav);
            SoundManager.Instance.PlaySound(heavyArrowImpact, transform);
        }

        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
            SoundManager.Instance.PlaySound(arrowBreak, transform);
        }
    }
}
