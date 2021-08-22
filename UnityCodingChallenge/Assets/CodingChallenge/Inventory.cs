using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType : int
{
    Grass, Flowers, Shrubs, Money
}
public abstract class InventoryData : MonoBehaviour
{
    public Dictionary<ItemType, int> itemToCount;
    public Dictionary<ItemType, string> itemToPath;

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
            itemToPath.Add(type, path);
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
                itemToPath.Remove(type);
            }
        }
        else
            Debug.LogWarning("Tried to remove inexisting item from inventory. " +
                "This, ideally, should never be called.");
    }

}
public class Inventory : InventoryController
{
    public Inventory(Dictionary<ItemType, int> countList, Dictionary<ItemType, string> pathList)
    {
        itemToCount = countList;
        itemToPath = pathList;
    }


}

