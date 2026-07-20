using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{
    public float destroyTime = 3f;
    public Vector3 offSet = new Vector3(0, 2, 0);
    public Vector3 randomisedIntensity = new Vector3(0.5f, 0, 0);
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        Destroy(gameObject, destroyTime);

        transform.localPosition += offSet;
        transform.localPosition += new Vector3(Random.Range(-randomisedIntensity.x, randomisedIntensity.x),Random.Range( -randomisedIntensity.y,randomisedIntensity.y), Random.Range( -randomisedIntensity.z, randomisedIntensity.z));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
