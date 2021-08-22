using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InventoryView
{
    [ContextMenu("Display Player Inventory")] public void DisplayPlayerInventory()
    {
        PersistentData.Debug();
        PersistentData.GetPlayerInventory(out Dictionary<ItemType, int> itemToCount, out Dictionary<ItemType, string> itemToPath);
        DisplayInventory(itemToCount, itemToPath);
    }
}
