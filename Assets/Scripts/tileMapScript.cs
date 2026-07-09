using UnityEngine;
using UnityEngine.UI;

public class tileMapScript : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] RawImage mapIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapIcon = GetComponentInChildren<RawImage>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            mapIcon.enabled = false;
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
