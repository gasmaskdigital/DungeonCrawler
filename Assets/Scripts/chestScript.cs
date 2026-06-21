using UnityEngine;

public class chestScript : MonoBehaviour
{

    public GameObject loot; // the contents of the chest
    [SerializeField] GameObject player;
    [SerializeField] Canvas canvas;
    [SerializeField] bool isPlayerClose;
    [SerializeField] bool isEmpty;
    [SerializeField] float lootOffset; // how far above the chest the loot spawns

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isEmpty = false;

        GameObject[] allLoot = Resources.LoadAll<GameObject>("LootObjects");
        loot = allLoot[Random.Range(0,allLoot.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose && !isEmpty) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canvas.gameObject.SetActive(false);
                Instantiate(loot, transform.position + Vector3.up * lootOffset, Quaternion.identity);
                isEmpty = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) isPlayerClose = true;
        if(!isEmpty) canvas.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player) isPlayerClose = false;
        canvas.gameObject.SetActive(false);
    }
}
