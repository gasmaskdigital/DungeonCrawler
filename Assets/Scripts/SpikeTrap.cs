using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private BoxCollider boxCollider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpikeTrapOn()
    {
        boxCollider.enabled = true;
    }

    public void SpikeTrapOff()
    {
        boxCollider.enabled = false;
    }

}
