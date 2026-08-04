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
    [SerializeField] public Sprite sword;
    [SerializeField] public Sprite bow;
    [SerializeField] public Sprite book;
    [SerializeField] public Sprite axe;
    [SerializeField] public Sprite claws;
    [SerializeField] public Sprite staff;
}