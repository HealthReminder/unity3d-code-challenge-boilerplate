using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warehouse : InventoryView
{
    private void OnEnable()
    {
        DisplayPlayerInventory();
    }
    [ContextMenu("Display Player Inventory")] public void DisplayPlayerInventory()
    {
        PersistentData.Debug();
        PersistentData.GetPlayerInventory(out Dictionary<ItemType, int> itemToCount, out Dictionary<ItemType, string> itemToPath);
        DisplayInventory(itemToCount, itemToPath);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
