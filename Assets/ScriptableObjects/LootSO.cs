using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Loot 
{
    public string name;
    public lootType type;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "LootSO", menuName = "Scriptable Objects/LootSO")]
public class LootSO : ScriptableObject
{
    public List<Loot> lootList;
}
