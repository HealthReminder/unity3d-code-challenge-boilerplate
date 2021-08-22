using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Item : int
{
    Money,Grass, Flowers, Shrubs
}
public abstract class InventoryData : MonoBehaviour
{
    public Dictionary<Item, int> itemToQuantity;

}
public abstract class InventoryController : InventoryData
{

    public void AddItem(int itemID)
    {
        Item item = (Item)itemID;
        if (itemToQuantity.ContainsKey(item))
        {
            itemToQuantity[item]++;
        }
        else
        {
            itemToQuantity.Add(item, 1);
        }
    }
    public void RemoveItem(int itemID)
    {
        Item item = (Item)itemID;
        if (itemToQuantity.ContainsKey(item))
        {
            itemToQuantity[item]--;
        }
        else
            Debug.LogWarning("Tried to remove inexisting item from inventory. " +
                "This, ideally, should never be called.");
    }

}
public class Inventory : InventoryController
{
    public Inventory()
    {
        itemToQuantity = new Dictionary<Item, int>();
    }


}

