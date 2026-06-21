using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lootHandler : MonoBehaviour
{
    public static List<GameObject> weapons = new();

    private void Start()
    {
        Object[] weaponPrefabs = Resources.LoadAll<GameObject>("LootObjects");

        foreach (GameObject weapon in weaponPrefabs)
        {
            Debug.Log(weapon.name);
            weapons.Add(weapon);
        }
    }
}
