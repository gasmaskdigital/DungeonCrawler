using UnityEngine;
using UnityEngine.UI;

public class tileMapScript : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] RawImage mapIcon;
    [SerializeField] BoxCollider triggerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapIcon = GetComponentInChildren<RawImage>();
        player = GameObject.FindGameObjectWithTag("Player");
        triggerCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (other.isTrigger)
            {
                Color mapColor = mapIcon.color;
                mapColor.a = 0.75f;
                mapIcon.color = mapColor;
            }
            else
            {
                mapIcon.enabled = false;
                Destroy(triggerCollider);
            }
        }
    }
}
