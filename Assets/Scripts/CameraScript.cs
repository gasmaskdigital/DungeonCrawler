
using Unity.Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private CinemachinePanTilt panTilt;
    public float rotateSpeed = 90f;

    void Start()
    {
        //panTilt = GetComponent<CinemachinePanTilt>();
    }

    void Update()
    {
        float direction = 0f;
        if (Input.GetKey(KeyCode.Q)) direction = -1f;
        if (Input.GetKey(KeyCode.E)) direction = 1f;

        panTilt.PanAxis.Value += direction * rotateSpeed * Time.deltaTime;
    }
}
