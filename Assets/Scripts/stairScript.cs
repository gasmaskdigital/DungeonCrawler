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

    private void OnTriggerStay(Collider other)
    {
        playerDist = (other.gameObject.transform.position - transform.position).magnitude;
        if (other.gameObject.CompareTag("Player") && playerDist < playerDistThreshold ) levelManager.increaseLevel();

    }
}
