using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable] public class ShopOption
{
    public ItemType item;
    public int quantity;
}
public class Shop : InventoryView
{
     public ShopOption[] options;
    private void OnEnable()
    {
        DisplayShopInventory();
    }
    [ContextMenu("Display Shop Inventory")] public void DisplayShopInventory()
    {
        Dictionary<ItemType, int> itemToCount = new Dictionary<ItemType, int>();
        Dictionary<ItemType, string> itemToPath = new Dictionary<ItemType, string>();
        for (int i = 0; i < options.Length; i++)
        {
            ItemType item = options[i].item;
            itemToCount.Add(item, options[i].quantity);
            itemToPath.Add(item, ($"Items/{item}"));
        }
        DisplayInventory(itemToCount, itemToPath);

    }
        //prefabPath = ;
    internal void SetMerchandise()
    {
        if(entries != null)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].SetMerchandise(true);
            }
        }
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
