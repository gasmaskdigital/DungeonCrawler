using UnityEngine;

public class stairScript : MonoBehaviour
{

    [SerializeField] float playerDist;
    [SerializeField] float playerDistThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !other.isTrigger) levelManager.increaseLevel();

    }
}
