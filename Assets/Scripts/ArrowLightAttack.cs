using UnityEngine;

public class ArrowLightAttack : MonoBehaviour
{
    private SphereCollider SphereCollider;
    private float speed = 15f;
    private GameObject player;
    private AttackHandler playerAttackHandler;
    Rigidbody myRigidBody;
    private VFXManager vfxManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackHandler = player.GetComponent<AttackHandler>();
        
        myRigidBody = GetComponent<Rigidbody>();

        myRigidBody.AddForce(transform.forward * speed, ForceMode.Impulse);

        vfxManager = FindAnyObjectByType<VFXManager>();
        
        //Destroy(gameObject, 2f);
    }

    private void OnTriggerStay(Collider other)
    {
        

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

        if (other.CompareTag("Terrain"))
        {
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
