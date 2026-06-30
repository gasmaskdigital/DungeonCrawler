using TMPro;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshProUGUI damageText;
    private float speed = 2f;
    private float lifeTime = 2f;
    private CinemachineOrbitalFollow orbitalFollow;


    public void Init(int damageDealt)
    {
        orbitalFollow = FindAnyObjectByType<CinemachineOrbitalFollow>();
        damageText.text = damageDealt.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timer =+ Time.deltaTime;

        transform.rotation = orbitalFollow.transform.rotation;

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if(timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    
}
