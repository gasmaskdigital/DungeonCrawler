using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public struct Loot 
{
    public string name;
    public lootType type;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "LootSO", menuName = "Scriptable Objects/LootSO")]
public class LootSO : ScriptableObject
{
    List<Loot> lootList;
}
