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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            Impact();
            ownerAttackHandler.TortHeavyImpact(playerStats);
            Destroy(gameObject, 1.5f);
        }

        if (other.CompareTag("Terrain"))
        {
            Impact();
            Destroy(gameObject, 1.5f);
        }
    }

    private void Impact()
    {
        vfxObject.gameObject.SetActive(false);
        impactVfxObject.gameObject.SetActive(true);
    }
}
