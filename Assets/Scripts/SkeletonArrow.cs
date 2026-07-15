using UnityEngine;



public class SkeletonArrow : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 1f;
    public GameObject owner;
    private EnemyAttackHandler ownerAttackHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ownerAttackHandler = owner.GetComponent<EnemyAttackHandler>();
        rb = GetComponent<Rigidbody>();

       // rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            ownerAttackHandler.SkeletonLightImpact(playerStats);

            Destroy(gameObject);
        }
        else if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }

    }
}
