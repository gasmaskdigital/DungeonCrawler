
using Unity.Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private CinemachineOrbitalFollow orbitalFollow;
    public float rotateSpeed = 90f;

    void Start()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        float direction = 0f;
        if (Input.GetKey(KeyCode.Q)) direction = -1f;
        if (Input.GetKey(KeyCode.E)) direction = 1f;

        orbitalFollow.HorizontalAxis.Value += direction * rotateSpeed * Time.deltaTime;
    }
}
