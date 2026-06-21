using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lootHandler : MonoBehaviour
{
    public static List<GameObject> weapons = new();
    public static List<GameObject> armour = new();

    private void Start()
    {
        Object[] weaponPrefabs = Resources.LoadAll<GameObject>("LootObjects/Weapons");
        Object[] armourPrefabs = Resources.LoadAll<GameObject>("LootObjects/Armour");

        foreach (GameObject weapon in weaponPrefabs) if(!weapons.Contains(weapon))weapons.Add(weapon);
        foreach (GameObject armourPiece in armourPrefabs) if(!armour.Contains(armourPiece)) armour.Add(armourPiece);
    }
}
