using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class chestScript : MonoBehaviour
{
    [SerializeField] LootSO allLoot;
    public Loot loot; // the contents of the chest
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

        List<Loot> lootList = allLoot.lootList;
        loot = lootList[Random.Range(0, lootList.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose && !isEmpty) 
        {
            canvas.transform.rotation = Camera.main.transform.rotation;

            if (Input.GetKeyDown(KeyCode.F))
            {
                canvas.gameObject.SetActive(false);
                Instantiate(loot.prefab, transform.position + Vector3.up * lootOffset, Quaternion.identity).GetComponent<lootScript>().isNewLoot = true;
                isEmpty = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            isPlayerClose = true;
            if (!isEmpty) canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger) isPlayerClose = false;
        canvas.gameObject.SetActive(false);
    }
}
