using UnityEngine;

public class enemySpawnerScript : MonoBehaviour
{

    [SerializeField] GameObject enemy;

    [SerializeField] float spawnRadius;
    [SerializeField] int spawnAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < spawnAmount; i++) spawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnEnemy() 
    {
        Vector3 offset = Random.onUnitSphere;
        Vector3 spawnPos = gameObject.transform.position + (new Vector3(offset.x,Mathf.Abs(offset.y), offset.z).normalized * spawnRadius);
        Instantiate(enemy, spawnPos, Quaternion.identity);
    }
}
