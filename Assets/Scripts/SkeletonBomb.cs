using System.Collections;
using UnityEngine;

public class SkeletonBomb : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private Rigidbody rb;
    private float speed = 100f;
    private float explosionRadius = 2f;
    public LayerMask playerMask;

    public GameObject owner;
    private EnemyAttackHandler enemyAttackHandler;

    [SerializeField] GameObject bombObject;
    [SerializeField] GameObject explosionObject;
    [SerializeField] AudioClip skeletonFuse;
    [SerializeField] AudioClip explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        enemyAttackHandler = owner.GetComponent<EnemyAttackHandler>();

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        StartCoroutine(Fuse());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Fuse()
    {
        SoundManager.Instance.PlaySound(skeletonFuse, transform, 0.8f);

        yield return new WaitForSeconds(3f);
        rb.isKinematic = true; // test
        bombObject.gameObject.SetActive(false);
        explosionObject.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound(explosion, transform, 0.8f);
        Explode();
        Destroy(gameObject, 1f);

    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, playerMask, QueryTriggerInteraction.Ignore);
        foreach(Collider c in colliders)
        {
            PlayerStats playerStats = c.GetComponent<PlayerStats>();

            enemyAttackHandler.SkeletonBombExplosion(playerStats);
        }
    }
}
