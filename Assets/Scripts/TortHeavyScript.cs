using Unity.Mathematics;
using UnityEngine;

public class TortHeavyScript : MonoBehaviour
{
    private Rigidbody rb;
    public float upForce = 8f;
    public float forwardForce = 10f;
    public GameObject owner;
    private EnemyAttackHandler ownerAttackHandler;
    [SerializeField] GameObject vfxObject;
    [SerializeField] GameObject impactVfxObject;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ownerAttackHandler = owner.GetComponent<EnemyAttackHandler>();

        Vector3 launchVelocity = transform.forward * forwardForce + Vector3.up * upForce;
        rb.AddForce(launchVelocity, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.useGravity = false;
            
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            Impact();
            ownerAttackHandler.TortHeavyImpact(playerStats);
            Destroy(gameObject);
        }

        if (other.CompareTag("Terrain"))
        {
            rb.useGravity = false;
            Impact();
            Destroy(gameObject, 2.2f);
        }
    }

    private void Impact()
    {
        vfxObject.gameObject.SetActive(false);
        Quaternion spawnRotation = Quaternion.Euler(-90f, 0, 0);
        Instantiate(impactVfxObject, transform.position, spawnRotation);
    }
}
