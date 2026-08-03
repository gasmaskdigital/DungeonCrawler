using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventorySlots;

    public List<inventoryItem> playerInventory = new List<inventoryItem>();


    public void AddItem(inventoryItem item)
    {
        playerInventory.Add(item);

        UpdateInventoryUI();
    }


    void UpdateInventoryUI()
    {
        // Hide all slots first
        foreach (Image slot in inventorySlots)
        {
            slot.enabled = false;
            slot.sprite = null;
        }


        // Show collected items
        for (int i = 0; i < playerInventory.Count; i++)
        {
            inventorySlots[i].sprite = playerInventory[i].sprite;
            inventorySlots[i].enabled = true;
        }
    }
}