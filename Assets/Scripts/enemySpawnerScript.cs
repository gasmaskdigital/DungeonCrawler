using UnityEngine;

public class enemySpawnerScript : MonoBehaviour
{

    [SerializeField] Object[] enemies;

    [SerializeField] float spawnRadius;
    [SerializeField] int minSpawnAmount;
    [SerializeField] int maxSpawnAmount;
    [SerializeField] AnimationCurve spawnCurveDist;
    int spawnAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = Resources.LoadAll("Enemies");

        int currentLevel = levelManager.currentLevel;
        if (currentLevel == 0) currentLevel++;
        minSpawnAmount = currentLevel;
        maxSpawnAmount = currentLevel * 2;
        setSpawnAmount();
        for (int i = 0; i < spawnAmount; i++) spawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnEnemy() 
    {
        Vector3 offset = Random.onUnitSphere;
        Vector3 spawnPos = gameObject.transform.position + (new Vector3(offset.x, Mathf.Abs(offset.y), offset.z).normalized * spawnRadius);
        int index = Random.Range(0, enemies.Length);
        Debug.Log("enemySpawnerIndex: " + index);
        GameObject enemy = (GameObject)enemies[index];
        Instantiate(enemy, spawnPos, Quaternion.identity).GetComponent<DummyAI>();
    }

    void setSpawnAmount() 
    {
        float curve = spawnCurveDist.Evaluate(Random.value);
        spawnAmount = Mathf.CeilToInt(Random.Range(minSpawnAmount * curve, maxSpawnAmount * curve));
        //Debug.Log(spawnAmount);
    }
}
