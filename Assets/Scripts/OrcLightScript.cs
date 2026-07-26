using UnityEngine;

public class OrcLightScript : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 12f;
    public GameObject owner;
    private EnemyAttackHandler ownerAttackHandler;
    [SerializeField] GameObject vfxObject;
    [SerializeField] GameObject impactVfxObject;
    private bool trigggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        ownerAttackHandler = owner.GetComponent<EnemyAttackHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!trigggered)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        trigggered = true;

        if (other.CompareTag("Player"))
        {

            Impact();
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            ownerAttackHandler.OrcLightImpact(playerStats);

            Destroy(gameObject, 1f);
        }
        else if (other.CompareTag("Terrain"))
        {
            Impact();
            Destroy(gameObject, 1f);
        }

    }

    private void Impact()
    {
        vfxObject.gameObject.SetActive(false);
        impactVfxObject.gameObject.SetActive(true);
    }
}
