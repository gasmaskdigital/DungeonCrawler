using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct inventoryItem
{
    public string name;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "InventorySpriteSO", menuName = "Scriptable Objects/InventorySpriteSO")]
public class InventorySpriteSO : ScriptableObject
{
    [SerializeField] public List<inventoryItem> inventoryItems;


    public inventoryItem Helmet;
    public inventoryItem Chestplate;
}
