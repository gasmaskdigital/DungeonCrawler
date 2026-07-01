using UnityEngine;

public class FireLIghtAttackScript : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private float speed = 15f;
    private GameObject player;
    private AttackHandler playerAttackHandler;
    private Vector3 tartgetScale = new Vector3(1.5f, 1.5f, 1.5f);
    private float scaleSpeed = 10f;
    private float explosionRadius = 0.75f;
    private bool triggered = false;
    public LayerMask enemyMask;
    Rigidbody myRigidBody;

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
        if (!triggered)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, tartgetScale, scaleSpeed * Time.deltaTime);

            if(transform.localScale == tartgetScale)
            {
                Explode();
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            triggered = true;
        }

        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask);

        foreach(Collider c in colliders)
        {
            EnemyStats enemyStats = c.GetComponent<EnemyStats>();
            AINavigation cAINav = c.GetComponent<AINavigation>();

            playerAttackHandler.FireLightAttackImpact(enemyStats, cAINav);
            Destroy(gameObject);
        }
    }
}
