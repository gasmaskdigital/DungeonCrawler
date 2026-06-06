using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f, 3.66f, -3.46f);
    private float smoothSpeed = 10f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
