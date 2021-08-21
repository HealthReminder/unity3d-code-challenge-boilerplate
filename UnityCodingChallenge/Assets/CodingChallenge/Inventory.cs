using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum InventoryItem
{
    Grass, Flowers, Shrubs
}
public abstract class InventoryData : MonoBehaviour
{
    public Dictionary<InventoryItem, int> itemToQuantity;

}
public abstract class InventoryController : InventoryData
{
    public void AddItem(int itemID)
    {
        InventoryItem item = (InventoryItem)itemID;
        if (itemToQuantity.ContainsKey(item))
        {
            itemToQuantity[item]++;
        } else
        {
            itemToQuantity.Add(item, 1);
        }
    }
    public void RemoveItem(int itemID)
    {
        InventoryItem item = (InventoryItem)itemID;
        if (itemToQuantity.ContainsKey(item))
        {
            itemToQuantity[item]--;
        }
        else
            Debug.LogWarning("Tried to remove inexisting item from inventory. " +
                "This, ideally, should never be called.");
    }
}
