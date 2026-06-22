using UnityEngine;

public class ArrowLightAttack : MonoBehaviour
{
    private SphereCollider SphereCollider;
    private float speed = 10f;
    private GameObject player;
    private AttackHandler playerAttackHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackHandler = player.GetComponent<AttackHandler>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }

        if (other.CompareTag("Terrain"))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
