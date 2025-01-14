using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType : int
{
    Grass, Flowers, Shrubs, Rake, Money
}
public abstract class InventoryData 
{
    public Dictionary<ItemType, int> itemToCount;

}
public abstract class InventoryController : InventoryData
{

    public void AddItem(PickableItem pickedItem, string path)
    {
        ItemType type = pickedItem.itemType;
        if (itemToCount.ContainsKey(type))
        {
            //Added a new object
            itemToCount[type]++;
        }
        else
        {
            //Added a new object
            itemToCount.Add(type, 1);
        }

    }
    public void RemoveItem(PickableItem droppedItem)
    {
        ItemType type = droppedItem.itemType;

        if (itemToCount.ContainsKey(type))
        {
            //Removed item
            itemToCount[type]--;
            if (itemToCount[type] <= 0)
            {
                //There is not more of this item left
                itemToCount.Remove(type);
            }
        }
        else
            Debug.LogWarning("Tried to remove inexisting item from inventory. " +
                "This, ideally, should never be called.");
    }

}
public class Inventory : InventoryController
{
    public Inventory(Dictionary<ItemType, int> countDic)
    {
        itemToCount = countDic;
    }


}

