using UnityEngine;

public class ArrowLightAttack : MonoBehaviour
{
    private SphereCollider SphereCollider;
    private float speed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        

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
