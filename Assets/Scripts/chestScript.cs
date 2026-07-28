using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class chestScript : MonoBehaviour
{
    [SerializeField] LootSO allLoot;
    public List<Loot> loot; // the contents of the chest
    [SerializeField] GameObject player;
    [SerializeField] Canvas canvas;
    [SerializeField] bool isPlayerClose;
    [SerializeField] bool isEmpty;
    [SerializeField] float lootOffset; // how far above the chest the loot spawns

    [Header("Sounds")]
    [SerializeField] AudioClip openChest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isEmpty = false;
        loot = getLoot();
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
                spawnLootObjects();
                SoundManager.Instance.PlaySound(openChest, transform);
            }
        }
    }

    private List<Loot> getLoot() 
    {
        List<Loot> lootList = allLoot.lootList;
        List<Loot> chestLoot = new();
        int lootAmount = Random.Range(1, 4);
        for (int i = 0; i < lootAmount; i++) chestLoot.Add(lootList[Random.Range(0, lootList.Count)]);
        return chestLoot;
    }

    private void spawnLootObjects() 
    {
        int lootAmount = loot.Count;
        List<Vector3> lootSpawnPos = new();
        Vector3 defaultPos = transform.position + Vector3.up * lootOffset;
        lootOffset *= 2;
        Debug.Log("lootAmount: " + lootAmount);
        switch (lootAmount) 
        {
            case (1):
                {
                    lootSpawnPos.Add(defaultPos); 
                    break;
                }
            case (2):
                {
                    lootSpawnPos.Add(defaultPos + Vector3.forward * lootOffset);
                    lootSpawnPos.Add(defaultPos + Vector3.back * lootOffset);
                    break;
                }
            case (3):
                {
                    lootSpawnPos.Add(defaultPos + Vector3.forward * lootOffset);
                    lootSpawnPos.Add(defaultPos + (-0.5f*Vector3.forward + (Mathf.Sqrt(3)/2f)*Vector3.left) * lootOffset);
                    lootSpawnPos.Add(defaultPos + (-0.5f*Vector3.forward - (Mathf.Sqrt(3)/2f)*Vector3.left) * lootOffset);

                    break;
                }
        }

        for (int i = 0; i < lootAmount; i++) 
        {
            Debug.Log(i);
            Instantiate(loot[i].prefab, lootSpawnPos[i], Quaternion.identity).GetComponent<lootScript>().isNewLoot = true;
        }

        isEmpty = true;
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
