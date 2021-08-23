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
        Inventory playerInventory = PersistentData.GetPlayerInventory();
        DisplayInventory(playerInventory.itemToCount);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
