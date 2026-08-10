using UnityEngine;

public class TrailerCam : MonoBehaviour
{
    public float speed = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
